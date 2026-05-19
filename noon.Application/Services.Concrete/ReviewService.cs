using noon.Application.DTOs.Product;
using noon.Application.DTOs.Review;
using noon.Application.Helpers;
using noon.Application.Service.Contract;
using noon.Domain.Models;

namespace noon.Application.Services.Concrete;

public class ReviewService:IReviewService   
{
    private readonly IUnitOfWork _unitOfWork;

    public ReviewService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<List<ReviewDto>> getAllReviewsAsync(int productId)
    {
        var result = await _unitOfWork.Reviews.getAllReviews(productId);
        
        return result.ToList();
    }

    public async Task<ReviewDto> addReviewAsync(createReviewDto createReviewDto,string currentUserId)
    {
        //check if user added reviewd Alredy
        
        var newReView = new ReView()
        {
            UserId = currentUserId,
            ProductId = createReviewDto.ProductId,
            ReviewRate = createReviewDto.ReviewRate,
            ReviewText =  createReviewDto.ReviewText,
        };
        
        await _unitOfWork.Reviews.addReviewAsync(newReView);

        var res = await _unitOfWork.SaveChangesAsync();
        
        if(res == 0)
            throw new ArgumentException("Cannot Add Review");
        
        return new ReviewDto()
        {
            Id =  newReView.Id,
            ReviewText =   newReView.ReviewText,
            ReviewRate =   newReView.ReviewRate,
        };

    }

    public async Task<Response> updateReview(int reviewId, updateReviewDto reviewDto)
    {
        var existReview = await _unitOfWork.Reviews.getReviewByIdAsync(reviewId);

        if (existReview == null)
        {
            return new Response
            {
                IsSuccess = false,
                Message = "Review not found"
            };
        }

        existReview.ReviewRate = reviewDto.ReviewRate;
        existReview.ReviewText = reviewDto.ReviewText;

        var result = await _unitOfWork.SaveChangesAsync();

        return new Response
        {
            IsSuccess = result > 0,
            Message = result > 0 ? "Updated successfully" : "Update failed"
        };
    }

    public async Task<Response> deleteReview(int reviewId,string currentUserId)
    {
        var existReview = await _unitOfWork.Reviews.getReviewByIdAsync(reviewId);
        
        if(existReview == null)
        {
            return new Response
            {
                IsSuccess = false,
                Message = "Review not found"
            };
        }
        
        if (existReview.UserId != currentUserId)
        {
            return new Response
            {
                IsSuccess = false,
                Message = "Not allowed"
            };
        }
        
        _unitOfWork.Reviews.deleteReview(existReview);
        
        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            return new Response()
            {
                IsSuccess = false,
                Message = "Cannot Delete Review",
            };
        }

        return new Response()
        {
            IsSuccess = true,
            Message = "Successfully Deleted Review",
        };
    }

    public async Task<ReviewDto> getReviewByIdAsync(int reviewId)
    {
        var result = await
            _unitOfWork.Reviews.getReviewByIdAsync(reviewId);

        if (result == null)
            throw new ArgumentNullException("Product Not Found");

        return new ReviewDto()
        {
            Id = result.Id,
            ReviewText = result.ReviewText,
            ReviewRate = result.ReviewRate,
        };
    }
}