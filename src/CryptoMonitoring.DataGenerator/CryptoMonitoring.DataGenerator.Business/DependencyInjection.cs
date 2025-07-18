using CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;
using CryptoMonitoring.DataGenerator.Business.Mapper.Profiles;
using CryptoMonitoring.DataGenerator.Business.Services.ExternalApis;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoMonitoring.DataGenerator.Business;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddHttpClient<IExternalApiHttpClient, ExternalApiHttpClient>();
        services.AddScoped<ICoinCapApiService, CoinCapApiService>();
        
        services.AddAutoMapper(cfg => { }, typeof(CryptoCurrencyProfile).Assembly);

        return services;
    }
}
