using CryptoMonitoring.DataGenerator.Business.Commands.DataGenerator;
using CryptoMonitoring.DataGenerator.Business.Commands.ExternalApis.CoinCapApi;
using CryptoMonitoring.DataGenerator.Business.Commands.ExternalApis.CoinGeckoApi;
using CryptoMonitoring.DataGenerator.Business.Interfaces.DataGenerator;
using CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;
using CryptoMonitoring.DataGenerator.Business.Interfaces.RabbitMQ;
using CryptoMonitoring.DataGenerator.Business.Services.DataGenerator;
using CryptoMonitoring.DataGenerator.Business.Services.ExternalApis;
using CryptoMonitoring.DataGenerator.Business.Services.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoMonitoring.DataGenerator.Business;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddHttpClient<IExternalApiHttpClient, ExternalApiHttpClient>();

        services.AddScoped<ICoinCapApiService, CoinCapApiService>();
        services.AddScoped<ICoinGeckoApiService, CoinGeckoApiService>();
        services.AddScoped<IDataGeneratorService, DataGeneratorService>();

        services.AddScoped<IRabbitMQPublisherService, RabbitMQPublisherService>();

        return services;
    }

    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddScoped<GenerateMarketDataCommand>();
        services.AddScoped<GeneratePriceHistoryDataCommand>();

        services.AddScoped<GetCryptoCurrenciesCoinCapCommand>();
        services.AddScoped<GetMarketDataCoinCapCommand>();
        services.AddScoped<GetPriceHistoryDataCoinCapCommand>();

        services.AddScoped<GetCryptoCurrenciesCoinGeckoCommand>();
        services.AddScoped<GetMarketDataCoinGeckoCommand>();
        services.AddScoped<GetPriceHistoryDataCoinGeckoCommand>();

        return services;
    }
}
