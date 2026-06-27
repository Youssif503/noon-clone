using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using noon.Application;
using noon.Application.Repository.Contract;
using StackExchange.Redis;
namespace noon.Infrastructure;
public class CacheService : ICacheService
{
    private readonly ILogger<CacheService> _logger;
    private readonly IDatabase _redis;
    private readonly IConfiguration _configuration;

    public CacheService(
        ILogger<CacheService> logger,
        IConnectionMultiplexer redis,
        IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _redis = redis.GetDatabase();
    }

    public async Task SetAsync<T>(string key, T value,TimeSpan TTL)
    {
        var serialized = JsonSerializer.Serialize(value);

        await _redis.StringSetAsync(key, serialized, TTL);
    }
    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _redis.StringGetAsync(key);

        if (value.IsNullOrEmpty)
        {
            _logger.LogInformation($"Cache miss for key: {key}");
            return default;
        }

        return JsonSerializer.Deserialize<T>(value.ToString());
    }
    public async Task RemoveAsync(string key)
    {
        await _redis.KeyDeleteAsync(key);
    }
}