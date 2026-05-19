namespace noon.Application.DTOs.Review;

public class createReviewDto
{
    public int ProductId { get; set; }
    public int ReviewRate {  get; set; }
    public string ReviewText { get; set; } = string.Empty;
}