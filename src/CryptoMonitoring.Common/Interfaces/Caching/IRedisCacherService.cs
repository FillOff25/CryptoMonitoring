using CryptoMonitoring.Models.Entities;

namespace CryptoMonitoring.Common.Interfaces.Caching;

public interface IRedisCacherService
{
    Task<T?> GetAsync<T>(string key);
    Task RemoveAsync(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
    string GenerateCryptoCurrencyKey(string name, string symbol);
}