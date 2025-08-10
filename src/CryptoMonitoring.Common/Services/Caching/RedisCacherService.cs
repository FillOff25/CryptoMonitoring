using CryptoMonitoring.Common.Interfaces.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace CryptoMonitoring.Common.Services.Caching;

public class RedisCacherService : IRedisCacherService
{
    private readonly IDistributedCache _cache;

    public RedisCacherService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var options = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(5),
        };

        var jsonData = JsonConvert.SerializeObject(value);
        await _cache.SetStringAsync(key, jsonData, options);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var jsonData = await _cache.GetStringAsync(key);
        return jsonData == null ? default : JsonConvert.DeserializeObject<T>(jsonData);
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }

    public string GenerateCryptoCurrencyKey(string name, string symbol)
    {
        return $"cc-{name}-{symbol}";
    }
}
