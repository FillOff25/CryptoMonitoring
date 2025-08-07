using CryptoMonitoring.Models.Entities;

namespace CryptoMonitoring.Common.Persistence.Interfaces;

public interface IUnitOfWork : IAsyncDisposable
{
    ICryptoCurrenciesRepository CryptoCurrencies { get; }
    IMarketDataRepository MarketData { get; }
    IGenericRepository<TechnicalIndicator, Guid> TechnicalIndicators { get; }
    IPriceHistoryDataRepository PriceHistoryData { get; }

    Task SaveAsync();
}