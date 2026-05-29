using noon.Application.DTOs;
using noon.Domain.Models;
namespace noon.Application.Repository.Contract;
public interface IProductRepository:IGenericRepository<Product>
{
    Task<ProductDto> getProductWithImagesByIdAsync(int productId);
    Task<List<ProductDto>> getProductsWithImagesAsync();
}