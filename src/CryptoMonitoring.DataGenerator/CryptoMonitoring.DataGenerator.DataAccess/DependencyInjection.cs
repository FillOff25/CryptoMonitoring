using CryptoMonitoring.DataGenerator.DataAccess.Databases;
using CryptoMonitoring.DataGenerator.DataAccess.Interfaces;
using CryptoMonitoring.DataGenerator.DataAccess.Repositories;
using CryptoMonitoring.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace CryptoMonitoring.DataGenerator.DataAccess;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        // Register DbContext in DI, configure and map custom enum types in database
        services.AddDbContext<CryptoMonitoringDataDbContext>(options =>
            options.UseNpgsql(configuration["CRYPTO_MONITORING_DATA_DB_CONNECTION_STRING"],
                o => o.MapEnum<IndicatorTypeEnum>("indicator_type_enum")));

        // Register repositories in DI
        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

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
