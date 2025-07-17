using CryptoMonitoring.Models.Entities;

namespace CryptoMonitoring.DataGenerator.Persistence.Interfaces;

public interface IUnitOfWork : IAsyncDisposable
{
    ICryptoCurrenciesRepository CryptoCurrencies { get; }
    IGenericRepository<MarketData, Guid> MarketDatas { get; }
    IGenericRepository<TechnicalIndicator, Guid> TechnicalIndicators { get; }

    Task SaveAsync();
}