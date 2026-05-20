using noon.Domain.Models;

namespace noon.Application.Repository.Contract;

public interface IImagesRepository:IGenericRepository<ProductImage>
{
    Task AddBulkAsync(List<ProductImage> images);
}