using Microsoft.EntityFrameworkCore;
using noon.Application.Repository.Contract;
using noon.Infrastructure.Data;
namespace noon.Infrastructure.Repositories;
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task addAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public async Task<T> getByIdAsync(object id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T>> getAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public void updateAsync(T entity)
    {
         _context.Set<T>().Update(entity);
    }

    public void deleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
}