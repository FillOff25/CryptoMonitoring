using CryptoMonitoring.Models.Entities;

namespace CryptoMonitoring.DataGenerator.Persistence.Interfaces;

public interface IMarketDataRepository : IGenericRepository<MarketData, Guid>
{
    Task<bool> IsUpdatedTodayAsync(Guid cryptoCurrencyId, DateTime timestamp);
    Task<MarketData?> GetByCryptoCurrencyIdAndTimestampAsync(Guid cryptoCurrencyId, DateTime timestamp);
    IQueryable<MarketData> GetByCryptoCurrencyId(Guid cryptoCurrencyId);
}