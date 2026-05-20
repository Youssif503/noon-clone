using noon.Application.DTOs.Review;
using noon.Application.Helpers;
namespace noon.Application.Service.Contract;

public interface IReviewService
{
    Task<List<ReviewDto>> getAllReviewsAsync(int productId);
    Task<ReviewDto> addReviewAsync(createReviewDto createReviewDto,string currentUserId);
    Task<Response> updateReview(int reviewId, updateReviewDto reviewDto);
    Task<Response> deleteReview(int reviewId,string currentUserId);
    Task<ReviewDto> getReviewByIdAsync(int reviewId);
}