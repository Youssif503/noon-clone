using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using noon.Application.Repository.Contract;
using StackExchange.Redis;

namespace noon.Infrastructure.Repositories;

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
        _redis = redis.GetDatabase();
        _configuration = configuration;
    }

    public async Task SetAsync<T>(string key, T value)
    {
        var serialized = JsonSerializer.Serialize(value);

        var ttl = TimeSpan.FromSeconds(
            _configuration.GetValue<int>("Redis:TTL"));

        await _redis.StringSetAsync(key, serialized, ttl);
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