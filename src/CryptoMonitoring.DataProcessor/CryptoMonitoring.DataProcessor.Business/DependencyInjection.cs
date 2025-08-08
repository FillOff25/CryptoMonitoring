using CryptoMonitoring.DataProcessor.Business.Commands.TechnicalIndicators;
using CryptoMonitoring.DataProcessor.Business.Interfaces;
using CryptoMonitoring.DataProcessor.Business.Interfaces.Processing;
using CryptoMonitoring.DataProcessor.Business.Services;
using CryptoMonitoring.DataProcessor.Business.Services.Processing;
using CryptoMonitoring.DataProcessor.Business.Services.RabbitMQ.CoinCap;
using CryptoMonitoring.DataProcessor.Business.Services.RabbitMQ.CoinGecko;
using CryptoMonitoring.DataProcessor.Business.Services.RabbitMQ.SyntheticData;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoMonitoring.DataProcessor.Business;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICoinCapProcessingService, CoinCapProcessingService>();
        services.AddScoped<ICoinGeckoProcessingService, CoinGeckoProcessingService>();
        services.AddScoped<ISyntheticDataProcessingService, SyntheticDataProcessingService>();
        services.AddScoped<ITechnicalIndicatorsService, TechnicalIndicatorsService>();

        services.AddHostedService<RabbitMQCoinCapCryptoCurrencyConsumerService>();
        services.AddHostedService<RabbitMQCoinCapMarketDataConsumerService>();
        services.AddHostedService<RabbitMQCoinCapPriceHistoryDataConsumerService>();

        services.AddHostedService<RabbitMQCoinGeckoCryptoCurrencyConsumerService>();
        services.AddHostedService<RabbitMQCoinGeckoMarketDataConsumerService>();
        services.AddHostedService<RabbitMQCoinGeckoPriceHistoryDataConsumerService>();

        services.AddHostedService<RabbitMQSyntheticMarketDataConsumerService>();
        services.AddHostedService<RabbitMQSyntheticPriceHistoryDataConsumerService>();

        return services;
    }

    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddScoped<CalculateTechnicalIndicatorCommand>();

        return services;
    }
}