using CryptoMonitoring.Common.Interfaces.Caching;
using CryptoMonitoring.Common.Services.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CryptoMonitoring.Common.Extensions;

public static class RedisExtensions
{
    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration["REDIS_CONFIGURATION"]!;
        });

        services.AddScoped<IRedisCacherService, RedisCacherService>();

        Log.Information("Redis configured");
        return services;
    }
}