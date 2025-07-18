using CryptoMonitoring.Models.Entities;

namespace CryptoMonitoring.DataGenerator.Persistence.Interfaces;

public interface IUnitOfWork : IAsyncDisposable
{
    ICryptoCurrenciesRepository CryptoCurrencies { get; }
    IMarketDatasRepository MarketDatas { get; }
    IGenericRepository<TechnicalIndicator, Guid> TechnicalIndicators { get; }
    IGenericRepository<PriceHistoryData, Guid> PriceHistoryDatas { get; }

    Task SaveAsync();
}