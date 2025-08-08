using CryptoMonitoring.Common.Persistence.Databases;
using CryptoMonitoring.Common.Persistence.Interfaces;
using CryptoMonitoring.Common.Persistence.Repositories;
using CryptoMonitoring.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace CryptoMonitoring.Common.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        // Register DbContext in DI, configure and map custom enum types in database
        services.AddDbContext<CryptoMonitoringDataDbContext>(options =>
            options.UseNpgsql(configuration["CRYPTO_MONITORING_DATA_DB_CONNECTION_STRING"],
                o => o.MapEnum<IndicatorTypeEnum>("indicator_type_enum")));

        // Register repositories in DI
        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        services.AddScoped<ICryptoCurrenciesRepository, CryptoCurrenciesRepository>();
        services.AddScoped<IMarketDataRepository, MarketDataRepository>();
        services.AddScoped<IPriceHistoryDataRepository, PriceHistoryDataRepository>();
        services.AddScoped<ITechnicalIndicatorsRepository, TechnicalIndicatorsRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        return services;
    }

    // Extension method for applying migrations in database
    public static async Task ApplyMigrationsAsync(this IHost app)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<CryptoMonitoringDataDbContext>();
            await dbContext.Database.MigrateAsync();

            Log.Information("Database migrations applied successfully");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while applying database migrations");
            throw;
        }
    }
}