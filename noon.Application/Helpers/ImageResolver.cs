using Microsoft.Extensions.Configuration;

namespace noon.Application.Helpers;

public class ImageResolver
{
    private readonly IConfiguration _configuration;

    public ImageResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string Resolve(string ImageUrl)
    {
        var BaseUrl =  _configuration.GetSection("BaseUrl").Value;
        return $"{BaseUrl}/images/{ImageUrl}";
    }
}