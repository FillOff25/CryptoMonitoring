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

    public async Task<bool> IsNameAndSymbolExistAsync(string name, string symbol)
    {
        return await DbSet.AsNoTracking()
            .AnyAsync(cc => cc.Name == name && cc.Symbol == symbol);
    }

    public async Task<bool> IsCoinCapIdExistAsync(string coinCapId)
    {
        return await DbSet.AsNoTracking()
            .AnyAsync(cc => cc.CoinCapId == coinCapId);
    }

    public async Task<bool> IsCoinGeckoIdExistAsync(string coinGeckoId)
    {
        return await DbSet.AsNoTracking()
            .AnyAsync(cc => cc.CoinGeckoId == coinGeckoId);
    }

    public async Task<CryptoCurrency?> GetByNameAndSymbolAsync(string name, string symbol)
    {
        return await DbSet.AsNoTracking()
            .FirstOrDefaultAsync(cc => cc.Name == name && cc.Symbol == symbol);
    }

    public async Task<CryptoCurrency?> GetByCoinCapIdAsync(string coinCapId)
    {
        return await DbSet.AsNoTracking()
            .FirstOrDefaultAsync(cc => cc.CoinCapId == coinCapId);
    }

    public async Task<CryptoCurrency?> GetByCoinGeckoIdAsync(string coinGeckoId)
    {
        return await DbSet.AsNoTracking()
            .FirstOrDefaultAsync(cc => cc.CoinGeckoId == coinGeckoId);
    }

    public IQueryable<string> GetCoinCapIds()
    {
        return DbSet.AsNoTracking()
            .Where(cc => cc.CoinCapId != null)
            .Select(cc => cc.CoinCapId!);
    }

    public IQueryable<string> GetCoinGeckoIds()
    {
        return DbSet.AsNoTracking()
            .Where(cc => cc.CoinGeckoId != null)
            .Select(cc => cc.CoinGeckoId!);
    }
}