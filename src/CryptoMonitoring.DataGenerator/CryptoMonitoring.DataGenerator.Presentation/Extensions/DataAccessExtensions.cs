using CryptoMonitoring.DataGenerator.DataAccess;
using CryptoMonitoring.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace CryptoMonitoring.DataGenerator.Presentation.Extensions;

public static class DataAccessExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection AddDatabases(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CryptoMonitoringDataDbContext>(options =>
            options.UseNpgsql(configuration["CRYPTO_MONITORING_DATA_DB_CONNECTION_STRING"],
                o => o.MapEnum<IndicatorTypeEnum>("indicator_type_enum")));

        return services;
    }
}
