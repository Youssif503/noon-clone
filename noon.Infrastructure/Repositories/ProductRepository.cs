using Microsoft.EntityFrameworkCore;
using noon.Application.DTOs;
using noon.Application.DTOs.Product;
using noon.Application.DTOs.Review;
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
    public async Task<ProductDto> getProductWithImagesByIdAsync(int productId)
    {
        return await _dbContext.Products
            .Select(p => new ProductDto()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.BasePrice,
                ProductImages = p.ProductImages.Select(i=> new ProductImageDto()
                {
                    ImageUrl = i.ImageUrl,
                    IsMain = i.isMain
                }).ToList(),
                Reviews = p.ReViews.Select(r=>new ReviewDto()
                {
                    Id = r.Id,
                    ReviewRate = r.ReviewRate,
                    ReviewText = r.ReviewText,
                }).ToList()
                
            })
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == productId);
    }
    
    public async Task<List<ProductDto>> getProductsWithImagesAsync()
    {
        return await _dbContext.Products
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.BasePrice,

                ProductImages = p.ProductImages.Select(i => new ProductImageDto
                {
                    ImageUrl = i.ImageUrl,
                    IsMain = i.isMain
                }).ToList(),

                Reviews = p.ReViews.Select(r => new ReviewDto
                {
                    Id = r.Id,
                    UserName = r.UserId,
                    ReviewRate = r.ReviewRate,
                    ReviewText = r.ReviewText
                }).ToList()
            })
            .AsNoTracking()
            .ToListAsync();
    }
    
}