using Microsoft.AspNetCore.Http;
using noon.Application.DTOs;
using noon.Application.DTOs.Product;
using noon.Application.Helpers;
namespace noon.Application.Service.Contract;
public interface IProductService
{
     Task<List<ProductDto>> getAllProductsAsync();
     Task<ProductDto> getProductByIdAsync(int productId);
     Task<ResponseProductDto> addProductAsync(createProductDto createProductDto,List<IFormFile> images);
     Task<Response> updateProduct(int productId, updateProductDto productDto);
     Task<Response> deleteProduct(int productId);
}