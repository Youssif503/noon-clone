using noon.Application.Repository.Contract;
using noon.Application.Service.Contract;
using noon.Domain.Models;
using noon.Infrastructure.Data;

namespace noon.Infrastructure.Repositories;

public class ImageRepository:GenericRepository<ProductImage>,IImagesRepository
{
    private readonly ApplicationDbContext _context;
    public ImageRepository(ApplicationDbContext context) 
        : base(context)
    {
        _context = context;
    }

    public async Task AddBulkAsync(List<ProductImage> images)
    {
        await _context.ProductImages.AddRangeAsync(images);
    }
}