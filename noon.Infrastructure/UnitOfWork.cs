using Microsoft.EntityFrameworkCore.Storage;
using noon.Application;
using noon.Application.Repository.Contract;
using noon.Domain.Models;
using noon.Infrastructure.Data;
using noon.Infrastructure.Repositories;

namespace noon.Infrastructure;

public class UnitOfWork : IUnitOfWork, IAsyncDisposable
{
    private IDbContextTransaction? _transaction;

    private readonly ApplicationDbContext _dbContext;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        Products = new ProductRepository(dbContext);

        Categories = new GenericRepository<Category>(dbContext);

        Reviews = new ReviewRepository(dbContext);

        Images = new ImageRepository(dbContext);
        
        Carts = new CartRepository(dbContext);
        RefreshTokens = new RefreshTokenRepository(dbContext);
    }

    public IProductRepository Products { get; private set; }
    public ICartRepository Carts { get; private set; }

    public IGenericRepository<Category> Categories { get; private set; }

    public IReviewRepository Reviews { get; private set; }

    public IRefreshTokenRepository RefreshTokens { get; private set; }

    public IImagesRepository Images { get; private set; }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        if (_transaction != null)
            return;

        _transaction =
            await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction == null)
            return;

        try
        {
            await _transaction.CommitAsync();
        }
        catch
        {
            await _transaction.RollbackAsync();
            throw;
        }
        finally
        {
            await _transaction.DisposeAsync();

            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction == null)
            return;

        await _transaction.RollbackAsync();

        await _transaction.DisposeAsync();

        _transaction = null;
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
        }

        await _dbContext.DisposeAsync();
    }
}