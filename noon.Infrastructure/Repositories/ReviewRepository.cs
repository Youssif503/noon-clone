using Microsoft.EntityFrameworkCore;
using noon.Application.DTOs.Review;
using noon.Application.Repository.Contract;
using noon.Domain.Models;
using noon.Infrastructure.Data;

namespace noon.Infrastructure.Repositories;

public class ReviewRepository:IReviewRepository
{
    private readonly ApplicationDbContext _context;
    public ReviewRepository(ApplicationDbContext dbContext)
    {
        _context = dbContext;
    }
    public async Task addReviewAsync(ReView review)
    {
        await _context.AddAsync(review);
    }

    public async Task<ReView> getReviewByIdAsync(int reviewId)
    {
        var result = await _context.ReViews.AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == reviewId);
        
        return result;
    }

    public async Task<IEnumerable<ReviewDto>> getAllReviews(int productId)
    {
        var result = await _context.ReViews
            .AsNoTracking()
            .Where(r => r.ProductId == productId)
            .Select(r => new ReviewDto
            {
                Id = r.Id,
                UserName = r.User.First_Name + " " + r.User.Last_Name,
                ReviewRate = r.ReviewRate,
                ReviewText = r.ReviewText
            })
            .ToListAsync();

        return result;
    }

    public void updateReview(ReView review)
    {
         _context.Update(review);
    }

    public void deleteReview(ReView review)
    {
         _context.Remove(review);
    }
}