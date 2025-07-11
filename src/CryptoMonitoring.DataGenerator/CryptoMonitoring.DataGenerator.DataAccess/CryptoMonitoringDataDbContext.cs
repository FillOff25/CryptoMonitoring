using CryptoMonitoring.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CryptoMonitoring.DataGenerator.DataAccess;

public class CryptoMonitoringDataDbContext : DbContext
{
    public CryptoMonitoringDataDbContext(DbContextOptions<CryptoMonitoringDataDbContext> options)
        : base(options)
    { }

    public DbSet<CryptoCurrency> CryptoCurrencies { get; set; }
    public DbSet<MarketData> MarketDatas { get; set; }
    public DbSet<TechnicalIndicator> TechnicalIndicators { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
