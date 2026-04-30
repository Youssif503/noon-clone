using noon.Application.Repository.Contract;
using noon.Domain.Models;
using noon.Infrastructure.Data;

namespace noon.Infrastructure.Repositories;

public class ProductRepository:GenericRepository<Product>,IProductRepository
{
    private readonly ApplicationDbContext _dbContext;
    public ProductRepository(ApplicationDbContext dbContext) 
        : base(dbContext)
    {
        _dbContext = dbContext; 
    }
}