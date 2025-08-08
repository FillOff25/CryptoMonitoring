using CryptoMonitoring.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CryptoMonitoring.Common.Persistence.Databases;

public class CryptoMonitoringDataDbContext : DbContext
{
    public CryptoMonitoringDataDbContext(DbContextOptions<CryptoMonitoringDataDbContext> options)
        : base(options)
    { }

    public DbSet<CryptoCurrency> CryptoCurrencies { get; set; }
    public DbSet<MarketData> MarketData { get; set; }
    public DbSet<PriceHistoryData> PriceHistoryData { get; set; }
    public DbSet<TechnicalIndicator> TechnicalIndicators { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
