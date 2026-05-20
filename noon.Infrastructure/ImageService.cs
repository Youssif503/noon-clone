using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using noon.Application.Service.Contract;
public class ImageService:IImageService
{
    private readonly IWebHostEnvironment _env;
    private readonly IConfiguration _configuration;
    //private readonly HashSet<string>? _allowedExtensions;
    public ImageService(IWebHostEnvironment env,IConfiguration configuration)
    {
        _configuration = configuration;
        _env = env; 
        
        // _allowedExtensions = _configuration
        //     .GetSection("AllowedExtensions")
        //     .Get<string[]>()!
        //     .Select(x => x.ToLower())
        //     .ToHashSet();
    }
    public async Task<string> SaveFileAsync(IFormFile imageFile)
    {
        if (imageFile == null)
        {
            throw new ArgumentNullException(nameof(imageFile));
        }
        
        var imagesPath = _env.ContentRootPath;
        var path = Path.Combine(imagesPath, "images"); //c:/blabla/blabla/images
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        
        var imageExt = Path.GetExtension(imageFile.FileName);
        
        // if (!_allowedExtensions.Contains(imageExt))
        // {
        //     throw  new Exception("Invalid image extension");
        // }

        var imageUrl = $"{Guid.NewGuid().ToString()}{imageExt}";
        var imageWithPath = Path.Combine(path, imageUrl);
        using (var stream = new FileStream(Path.Combine(path, imageUrl), FileMode.Create))
        {
            await imageFile.CopyToAsync(stream);
        }

        return imageUrl;
    }

    public void DeleteFile(string imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl))
        {
            throw new ArgumentNullException(nameof(imageUrl));
        }
        var contentPath = _env.ContentRootPath;
        var path = Path.Combine(contentPath, $"images", imageUrl);

        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Invalid file path");
        }
        File.Delete(path);
    }
}