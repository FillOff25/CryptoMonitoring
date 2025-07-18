using CryptoMonitoring.DataGenerator.Persistence.Databases;
using CryptoMonitoring.DataGenerator.Persistence.Interfaces;
using CryptoMonitoring.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoMonitoring.DataGenerator.Persistence.Repositories;

public class CryptoCurrenciesRepository : GenericRepository<CryptoCurrency, Guid>, ICryptoCurrenciesRepository
{
    public CryptoCurrenciesRepository(CryptoMonitoringDataDbContext dbContext)
        : base(dbContext)
    { }

    public async Task<bool> IsNameExistAsync(string name)
    {
        return await DbSet.AnyAsync(cc => cc.Name == name);
    }

    public async Task<CryptoCurrency?> GetByNameAsync(string name)
    {
        return await DbSet.AsNoTracking()
            .FirstOrDefaultAsync(cc => cc.Name == name);
    }
}