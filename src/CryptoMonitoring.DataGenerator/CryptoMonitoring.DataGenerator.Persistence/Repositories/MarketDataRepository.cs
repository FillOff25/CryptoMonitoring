using CryptoMonitoring.DataGenerator.Persistence.Databases;
using CryptoMonitoring.DataGenerator.Persistence.Interfaces;
using CryptoMonitoring.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoMonitoring.DataGenerator.Persistence.Repositories;

public class MarketDataRepository : GenericRepository<MarketData, Guid>, IMarketDataRepository
{
    public MarketDataRepository(CryptoMonitoringDataDbContext dbContext)
        : base(dbContext)
    { }

    public async Task<bool> IsUpdatedTodayAsync(Guid cryptoCurrencyId, DateTime timestamp)
    {
        return await DbSet.AnyAsync(md =>
            md.CryptoCurrencyId == cryptoCurrencyId &&
            md.Timestamp.Date == timestamp.Date);
    }

    public async Task<MarketData?> GetByCryptoCurrencyIdAndTimestampAsync(Guid cryptoCurrencyId, DateTime timestamp)
    {
        return await DbSet.AsNoTracking()
            .FirstOrDefaultAsync(md =>
                md.CryptoCurrencyId == cryptoCurrencyId &&
                md.Timestamp.Date == timestamp.Date);
    }

    public IQueryable<MarketData> GetByCryptoCurrencyId(Guid cryptoCurrencyId)
    {
        return DbSet.AsNoTracking()
            .Where(md => md.CryptoCurrencyId == cryptoCurrencyId);
    }
}