using CryptoMonitoring.Models.Entities;

namespace CryptoMonitoring.DataGenerator.Persistence.Interfaces;

public interface IMarketDataRepository : IGenericRepository<MarketData, Guid>
{
    Task<bool> IsUpdatedTodayAsync(MarketData marketData);
    Task<MarketData?> GetByCryptoCurrencyIdAndTimestamp(Guid cryptoCurrencyId, DateTime timestamp);
}