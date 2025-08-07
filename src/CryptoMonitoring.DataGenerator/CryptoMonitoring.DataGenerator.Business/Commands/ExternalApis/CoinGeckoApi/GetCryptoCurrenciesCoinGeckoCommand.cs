using CryptoMonitoring.Common.Extensions;
using CryptoMonitoring.Common.Interfaces;
using CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;
using CryptoMonitoring.DataGenerator.Business.Interfaces.RabbitMQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CryptoMonitoring.DataGenerator.Business.Commands.ExternalApis.CoinGeckoApi;

public class GetCryptoCurrenciesCoinGeckoCommand : ICommand<object?, IActionResult>
{
    private readonly ICoinGeckoApiService _coinGeckoApiService;
    private readonly IRabbitMQPublisherService _rabbitMQPublisherService;
    private readonly string _routingKey;

    public GetCryptoCurrenciesCoinGeckoCommand(
        ICoinGeckoApiService coinGeckoApiService, 
        IRabbitMQPublisherService rabbitMQPublisherService,
        IConfiguration configuration)
    {
        _coinGeckoApiService = coinGeckoApiService;
        _rabbitMQPublisherService = rabbitMQPublisherService;
        _routingKey = configuration["RABBITMQ_COINGECKO_CRYPTOCURRENCY_PROCESSOR_QUEUE_ROUTING_KEY"]!;
    }

    public async Task<IActionResult> ExecuteAsync(object? request = null)
    {
        var response = await _coinGeckoApiService.GetCryptoCurrencyAsync();
        await _rabbitMQPublisherService.PublishAsync(response, _routingKey);
        
        return new object().ToHttpResponse("Crypto currency data from CoinGecko Api fetched successfully", 200);
    }
}
