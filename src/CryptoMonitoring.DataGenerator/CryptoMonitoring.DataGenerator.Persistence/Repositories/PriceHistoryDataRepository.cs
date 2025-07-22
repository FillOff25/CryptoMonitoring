using CryptoMonitoring.DataGenerator.Persistence.Databases;
using CryptoMonitoring.DataGenerator.Persistence.Interfaces;
using CryptoMonitoring.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoMonitoring.DataGenerator.Persistence.Repositories;

public class PriceHistoryDataRepository : GenericRepository<PriceHistoryData, Guid>, IPriceHistoryDataRepository
{
    public PriceHistoryDataRepository(CryptoMonitoringDataDbContext dbContext)
        : base(dbContext)
    { }

    public async Task<bool> IsExist(Guid cryptoCurrencyId, DateTime timestamp)
    {
        return await DbSet.AnyAsync(phd =>
            phd.CryptoCurrencyId == cryptoCurrencyId && phd.Timestamp == timestamp);
    }
}
