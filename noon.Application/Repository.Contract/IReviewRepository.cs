using noon.Application.DTOs.Review;
using noon.Domain.Models;

namespace noon.Application.Repository.Contract;

public interface IReviewRepository
{
    Task addReviewAsync(ReView review);
    Task<ReView> getReviewByIdAsync(int reviewId);
    Task<IEnumerable<ReviewDto>> getAllReviews(int productId);
    void updateReview(ReView review);
    void deleteReview(ReView review);
}