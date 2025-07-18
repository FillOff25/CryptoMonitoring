using CryptoMonitoring.DataGenerator.Persistence.Databases;
using CryptoMonitoring.DataGenerator.Persistence.Interfaces;
using CryptoMonitoring.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoMonitoring.DataGenerator.Persistence.Repositories;

public class MarketDatasRepository : GenericRepository<MarketData, Guid>, IMarketDatasRepository
{
    public MarketDatasRepository(CryptoMonitoringDataDbContext dbContext)
        : base(dbContext)
    { }

    public async Task<bool> IsUpdatedTodayAsync(MarketData marketData)
    {
        return await DbSet.AnyAsync(md =>
            md.CryptoCurrencyId == marketData.CryptoCurrencyId &&
            md.Timestamp.Date == marketData.Timestamp.Date);
    }

    public async Task<MarketData?> GetByCryptoCurrencyIdAndTimestamp(Guid cryptoCurrencyId, DateTime timestamp)
    {
        return await DbSet.AsNoTracking()
            .FirstOrDefaultAsync(md =>
                md.CryptoCurrencyId == cryptoCurrencyId &&
                md.Timestamp.Date == timestamp.Date);
    }
}