using noon.Application.DTOs;
using noon.Application.Helpers;
using noon.Domain.Models;

namespace noon.Application.Service.Contract;

public interface IProductService
{
     Task<List<ProductDto>> getAllProductsAsync();
     Task<ProductDto> getProductByIdAsync(int productId);
     Task<Response> addProductAsync(createProductDto createProductDto);
     Task<Response> updateProduct(int productId, updateProductDto productDto);
     Task<Response> deleteProduct(int productId);
}