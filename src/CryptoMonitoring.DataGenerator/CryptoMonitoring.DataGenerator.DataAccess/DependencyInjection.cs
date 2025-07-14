using CryptoMonitoring.DataGenerator.DataAccess.Databases;
using CryptoMonitoring.DataGenerator.DataAccess.Interfaces;
using CryptoMonitoring.DataGenerator.DataAccess.Repositories;
using CryptoMonitoring.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CryptoMonitoring.DataGenerator.DataAccess;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

        return services;
    }

    public static IServiceCollection AddDatabases(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CryptoMonitoringDataDbContext>(options =>
            options.UseNpgsql(configuration["CRYPTO_MONITORING_DATA_DB_CONNECTION_STRING"],
                o => o.MapEnum<IndicatorTypeEnum>("indicator_type_enum")));

        return services;
    }

    public static async Task ApplyMigrationsAsync(this IHost app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CryptoMonitoringDataDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}
