using CryptoMonitoring.Common.Interfaces.Entities;
using CryptoMonitoring.Common.Services.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoMonitoring.Common.Extensions;

public static class CommonExtensions
{
    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        services.AddScoped<ICryptoCurrencyService, CryptoCurrencyService>();

        return services;
    }
}
