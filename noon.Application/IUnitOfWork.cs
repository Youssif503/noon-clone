using noon.Application.Repository.Contract;
using noon.Domain.Models;

namespace noon.Application;
public interface IUnitOfWork
{
    IProductRepository Products { get; }
    IGenericRepository<Category>  Categories { get; }
    IReviewRepository Reviews { get; }
    Task<int> SaveChangesAsync();

}