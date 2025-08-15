using CryptoMonitoring.Common.Persistence.Databases;
using CryptoMonitoring.Common.Persistence.Interfaces;
using CryptoMonitoring.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoMonitoring.Common.Persistence.Repositories;

public class PriceHistoryDataRepository : GenericRepository<PriceHistoryData, Guid>, IPriceHistoryDataRepository
{
    public PriceHistoryDataRepository(CryptoMonitoringDataDbContext dbContext)
        : base(dbContext)
    { }

    public async Task<bool> IsExist(Guid cryptoCurrencyId, DateTime timestamp)
    {
        return await DbSet.AnyAsync(phd =>
            phd.CryptoCurrencyId == cryptoCurrencyId && phd.Timestamp.Date == timestamp.Date);
    }

    public IQueryable<PriceHistoryData> GetByCryptoCurrencyId(Guid cryptoCurrencyId)
    {
        return DbSet.AsNoTracking()
            .Where(phd => phd.CryptoCurrencyId == cryptoCurrencyId);
    }
}
