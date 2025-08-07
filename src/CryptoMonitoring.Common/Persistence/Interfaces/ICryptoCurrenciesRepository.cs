using CryptoMonitoring.Models.Entities;

namespace CryptoMonitoring.Common.Persistence.Interfaces;

public interface ICryptoCurrenciesRepository : IGenericRepository<CryptoCurrency, Guid>
{
    Task<bool> IsNameAndSymbolExistAsync(string name, string symbol);
    Task<bool> IsCoinCapIdExistAsync(string name);
    Task<bool> IsCoinGeckoIdExistAsync(string coinGeckoId);
    Task<CryptoCurrency?> GetByNameAndSymbolAsync(string name, string symbol);
    Task<CryptoCurrency?> GetByCoinCapIdAsync(string name);
    Task<CryptoCurrency?> GetByCoinGeckoIdAsync(string coinGeckoId);
    IQueryable<string> GetCoinCapIds();
    IQueryable<string> GetCoinGeckoIds();
}