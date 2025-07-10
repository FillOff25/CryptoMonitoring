using CryptoMonitoring.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CryptoMonitoring.DataGenerator.DataAccess;

public class CryptoMonitoringDataDbContext : DbContext
{
    public DbSet<CryptoCurrency> CryptoCurrencies { get; set; }
    public DbSet<Market> Markets { get; set; }
    public DbSet<PriceHistory> PriceHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
