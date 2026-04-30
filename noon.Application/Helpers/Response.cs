namespace noon.Application.Helpers;

public class Response
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public List<string?> Errors { get; set; }
}