using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using noon.Application.DTOs;
using noon.Application.DTOs.Product;
using noon.Application.Helpers;
using noon.Application.Repository.Contract;
using noon.Application.Service.Contract;
using noon.Domain.Models;

namespace noon.Application.Services.Concrete;

public class ProductService:IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;
    private readonly ImageResolver _imageResolver;
    public ProductService(IUnitOfWork unitOfWork , 
        IImageService imageService,
        ImageResolver imageResolver)
    {
        _unitOfWork = unitOfWork;
        _imageService = imageService;
        _imageResolver = imageResolver;
    }
    
    public async Task<List<ProductDto>> getAllProductsWithImagesAsync()
    {
        var Products = await _unitOfWork.Products.getProductsWithImagesAsync();
        
        if(Products==null)
            throw new NullReferenceException(nameof(Products));

        foreach (var product in Products)
        {
            foreach (var image in product.ProductImages)
            {
                image.ImageUrl = _imageResolver.Resolve(image.ImageUrl);
            }
        }
        return Products;
        
    }
    
    public async Task<ProductDto> getProductWithImagesByIdAsync(int productId)
    {
        var product = await _unitOfWork.Products.getProductWithImagesByIdAsync(productId);
        
       if(product==null)
           throw new NullReferenceException("Not Found Ya 7maaaaar");

       foreach (var image in product.ProductImages)
       {
           image.ImageUrl = _imageResolver.Resolve(image.ImageUrl);
       }
       
        return product;
    }

    public async Task<ResponseProductDto> addProductAsync(createProductDto createProductDto,List<IFormFile> images)
    {
        if (createProductDto.StockCount <= 0)
            throw new ArgumentException(nameof(createProductDto.StockCount));
        
        if (images == null || images.Count == 0)
            throw new ArgumentException("Images are required");
        
        var uploadedFiles = new List<string>();
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            Product newProduct = new Product
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                StockCount = createProductDto.StockCount,
                BasePrice = createProductDto.Price,
                CategoryId = createProductDto.CategoryId,
            };

            await _unitOfWork.Products.addAsync(newProduct);
            await _unitOfWork.SaveChangesAsync();
            
            List<ProductImage> productimages = new List<ProductImage>();

            foreach (var image in images)
            {
                var imageUrl = await _imageService.SaveFileAsync(image);
                uploadedFiles.Add(imageUrl);
                ProductImage newImage = new ProductImage()
                {
                    ImageUrl = imageUrl,
                    ProductId = newProduct.Id,
                };
                productimages.Add(newImage);
            }
            productimages[0].isMain = true;
            
            await _unitOfWork.Images.AddBulkAsync(productimages);
            
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();
            List<string> response = new();
            foreach (var image in uploadedFiles)
            {
                response.Add($"http://localhost:5250/images/{image}");
            }
            
            return new ResponseProductDto
            {
                Name =  newProduct.Name,
                Id =  newProduct.Id,
                Description =  newProduct.Description,
                StockCount =   newProduct.StockCount,
                BasePrice =  newProduct.BasePrice,
                Images = response
            };

        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            
            foreach (var file in uploadedFiles)
            {
                _imageService.DeleteFile(file);
            }
            
            throw;
        }
    }

    public async Task<Response> deleteProduct(int productId)
    {
        var product = await _unitOfWork.Products.getByIdAsync(productId);
        if (product == null)
            return new Response
            {
                IsSuccess = false,
                Message =  "Product not found"
            };
        
        _unitOfWork.Products.delete(product);
        int isSuccess = await _unitOfWork.SaveChangesAsync();
        
        if (isSuccess == 0)
        {
            return new Response
            {
                IsSuccess = false,
                Message = "Product not deleted"
            };
        }
        
        
        return new Response
        {
            IsSuccess = true,
            Message = "Product deleted successfully"
        };
    }
    
    public async Task<Response> updateProduct(int productId, updateProductDto productDto)
    {
        var Product = await _unitOfWork.Products.getByIdAsync(productId);
        if (Product == null)
            return new Response
            {
                IsSuccess = false,
                Message = "Product not found"
            };
        
        Product.BasePrice = productDto.Price;
        Product.Name = productDto.Name;
        Product.Description = productDto.Description;
        _unitOfWork.Products.update(Product);

        var isSuceess = await _unitOfWork.SaveChangesAsync();
        if (isSuceess == 0)
        {
            return new Response
            {
                IsSuccess = false,
                Message = "Product not updated"
            };
        }
        
        return new Response
        {
            IsSuccess = true,
            Message = "Product updated successfully"
        };
    }

}