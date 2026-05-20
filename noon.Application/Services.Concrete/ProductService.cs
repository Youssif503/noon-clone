using Microsoft.AspNetCore.Http;
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
    public ProductService(IUnitOfWork unitOfWork , IImageService imageService)
    {
        _unitOfWork = unitOfWork;
        _imageService = imageService;
    }
    public async Task<List<ProductDto>> getAllProductsAsync()
    {
        var Products = await _unitOfWork.Products.getAllAsync();
        var productDtos = Products.
            Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.BasePrice,
                Description = p.Description,
                ProductImages = p.ProductImages
                
            })
            .ToList();
        
        return productDtos;
        
    }

    public async Task<ProductDto> getProductByIdAsync(int productId)
    {
        var product = await _unitOfWork.Products.getByIdAsync(productId);
        ProductDto responseProduct = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.BasePrice,
            ProductImages = product.ProductImages,
            ReViews = product.ReViews
        };
        
        return responseProduct;
    }

    public async Task<ResponseProductDto> addProductAsync(createProductDto createProductDto,List<IFormFile> images)
    {
        if (createProductDto.StockCount <= 0)
            return null;
        
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
            await _unitOfWork.Images.AddBulkAsync(productimages);
            
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();
            
            return new ResponseProductDto
            {
                Name =  newProduct.Name,
                Id =  newProduct.Id,
                Description =  newProduct.Description,
                StockCount =   newProduct.StockCount,
                BasePrice =  newProduct.BasePrice,
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
        await _unitOfWork.SaveChangesAsync();
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

        await _unitOfWork.SaveChangesAsync();

        return new Response
        {
            IsSuccess = true,
            Message = "Product updated successfully"
        };
    }

}