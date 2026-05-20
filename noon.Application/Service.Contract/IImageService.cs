using Microsoft.AspNetCore.Http;

namespace noon.Application.Service.Contract;

public interface IImageService
{
    Task<string> SaveFileAsync(IFormFile imageFile);
    void DeleteFile(string imageUrl);
}