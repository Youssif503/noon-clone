using noon.Application.Repository.Contract;
using noon.Domain.Models;
namespace noon.Application;
public interface IUnitOfWork
{
    IProductRepository Products { get; }
    IGenericRepository<Category>  Categories { get; }
    ICartRepository Carts { get; }
    IImagesRepository Images { get; }
    IReviewRepository Reviews { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();

}