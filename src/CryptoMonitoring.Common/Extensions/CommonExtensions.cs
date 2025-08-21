using CryptoMonitoring.Common.Interfaces.Auth;
using CryptoMonitoring.Common.Interfaces.Entities;
using CryptoMonitoring.Common.Services.Auth;
using CryptoMonitoring.Common.Services.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CryptoMonitoring.Common.Extensions;

public static class CommonExtensions
{
    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        services.AddScoped<ICryptoCurrencyService, CryptoCurrencyService>();
        services.AddScoped<IJwtService, JwtService>();

        return services;
    }

    public static async Task SeedDataAsync(this IHost app, IConfiguration configuration)
    {
        await using var scope = app.Services.CreateAsyncScope();
        await SeedDataService.SeedDataAsync(scope, configuration);
    }
}