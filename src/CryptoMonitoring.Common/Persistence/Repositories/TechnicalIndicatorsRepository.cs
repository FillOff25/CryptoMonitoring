using CryptoMonitoring.Common.Persistence.Databases;
using CryptoMonitoring.Common.Persistence.Interfaces;
using CryptoMonitoring.Models.Entities;
using CryptoMonitoring.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace CryptoMonitoring.Common.Persistence.Repositories;

public class TechnicalIndicatorsRepository : GenericRepository<TechnicalIndicator, Guid>, ITechnicalIndicatorsRepository
{
    public TechnicalIndicatorsRepository(CryptoMonitoringDataDbContext dbContext)
        : base(dbContext)
    { }

    public async Task<bool> IsExist(Guid cryptoCurrencyId, IndicatorTypeEnum type, DateTime timestamp)
    {
        return await DbSet.AnyAsync(ti => 
            ti.CryptoCurrencyId == cryptoCurrencyId && 
            ti.IndicatorType == type && 
            ti.Timestamp.Date == timestamp.Date);
    }

    public IQueryable<TechnicalIndicator> GetByCryptoCurrencyId(Guid cryptoCurrencyId)
    {
        return DbSet.AsNoTracking()
            .Where(ti => ti.CryptoCurrencyId == cryptoCurrencyId);
    }
}