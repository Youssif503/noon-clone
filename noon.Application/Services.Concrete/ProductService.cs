using noon.Application.DTOs;
using noon.Application.Helpers;
using noon.Application.Repository.Contract;
using noon.Application.Service.Contract;
using noon.Domain.Models;

namespace noon.Application.Services.Concrete;

public class ProductService:IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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

    public async Task<Response> addProductAsync(createProductDto createProductDto)
    {
        if (createProductDto.StockCount < 0)
            return new Response
            {
                IsSuccess =  false,
                Message = "Stock count must be greater than or equal to zero"
            };
        
        Product newProduct = new Product
        {
            Name = createProductDto.Name,
            Description = createProductDto.Description,
            StockCount = createProductDto.StockCount,
            BasePrice = createProductDto.Price,
            ProductImages = createProductDto.ProductImages
        };
        
        await _unitOfWork.Products.addAsync(newProduct);
        await _unitOfWork.SaveChangesAsync();
        return new Response
        {
            IsSuccess = true,
            Message = "Product added successfully"
        };

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
        
        _unitOfWork.Products.deleteAsync(product);
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
        _unitOfWork.Products.updateAsync(Product);

        await _unitOfWork.SaveChangesAsync();

        return new Response
        {
            IsSuccess = true,
            Message = "Product updated successfully"
        };
    }

}