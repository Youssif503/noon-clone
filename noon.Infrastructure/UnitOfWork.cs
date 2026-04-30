using noon.Application;
using noon.Application.Repository.Contract;
using noon.Infrastructure.Data;
using noon.Infrastructure.Repositories;

namespace noon.Infrastructure;

public class UnitOfWork : IUnitOfWork ,IDisposable
{
    private readonly ApplicationDbContext _dbContext;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        Products = new ProductRepository(dbContext);
    }
    public IProductRepository Products { get; private set; }
    
    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
    
    public void Dispose()
    {
        _dbContext.Dispose();
    }
}