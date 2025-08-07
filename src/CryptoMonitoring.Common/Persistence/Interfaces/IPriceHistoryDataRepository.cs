using CryptoMonitoring.Models.Entities;

namespace CryptoMonitoring.Common.Persistence.Interfaces;

public interface IPriceHistoryDataRepository : IGenericRepository<PriceHistoryData, Guid>
{
    Task<bool> IsExist(Guid cryptoCurrencyId, DateTime timestamp);
    IQueryable<PriceHistoryData> GetByCryptoCurrencyId(Guid cryptoCurrencyId);
}