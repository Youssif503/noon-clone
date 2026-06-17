namespace noon.Application.Repository.Contract;
public interface ICacheService
{
    Task SetAsync<T>(string key, T value);
    Task<T> GetAsync<T>(string key);
    Task RemoveAsync(string key);
}