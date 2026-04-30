using noon.Application.Repository.Contract;

namespace noon.Application;
public interface IUnitOfWork
{
    IProductRepository Products { get; }
    Task<int> SaveChangesAsync();

}