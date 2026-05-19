namespace noon.Application.DTOs.Review;

public class ReviewDto
{
    public string UserName { get; set; }
    public int Id { get; set; }
    public int ReviewRate {  get; set; }
    public string ReviewText { get; set; } = string.Empty;
}