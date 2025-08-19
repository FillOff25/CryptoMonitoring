using CryptoMonitoring.Models.Entities;

namespace CryptoMonitoring.Common.Interfaces.Entities;

public interface ICryptoCurrencyService
{
    Task<CryptoCurrency> GetCryptoCurrencyWithCache(string name, string symbol);
}