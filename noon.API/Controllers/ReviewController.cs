using Microsoft.AspNetCore.Mvc;
using noon.Application.DTOs.Review;
using noon.Application.Service.Contract;
namespace noon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("product/{productId:int}")]
        public async Task<IActionResult> GetAll(int productId)
        {
            var result = await _reviewService.getAllReviewsAsync(productId);
            return Ok(result);
        }
        
        [HttpGet("{reviewId:int}")]
        public async Task<IActionResult> GetById(int reviewId)
        {
            var result = await _reviewService.getReviewByIdAsync(reviewId);

            if (result == null)
                return NotFound("Review not found");

            return Ok(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> Add(createReviewDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _reviewService.addReviewAsync(dto, userId);

            if (result == null)
                return BadRequest("You already reviewed this product or failed to add review");

            return CreatedAtAction(
                nameof(GetById),
                new { reviewId = result.Id },
                result
            );
        }
        
        [HttpPut("{reviewId:int}")]
        public async Task<IActionResult> Update(int reviewId, updateReviewDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _reviewService.updateReview(reviewId, dto);

            if (!result.IsSuccess)
                return NotFound(result.Message);

            return NoContent();
        }
        
        [HttpDelete("{reviewId:int}")]
        public async Task<IActionResult> Delete(int reviewId)
        {
            var userId = User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _reviewService.deleteReview(reviewId, userId);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return NoContent();
        }
    }
}