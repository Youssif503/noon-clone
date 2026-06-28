namespace noon.Application.Helpers;

public class Jwt
{
    public string? Audience { get; set; }
    public string? Issuer { get; set; }
    public string? SecretKey { get; set; }
    public double? DurationInHour { get; set; }
    public int? RefreshTokenDurationInDays { get; set; }
}