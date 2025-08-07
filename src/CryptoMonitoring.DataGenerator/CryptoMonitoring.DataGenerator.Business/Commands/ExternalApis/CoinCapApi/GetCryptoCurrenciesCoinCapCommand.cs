using CryptoMonitoring.Common.Extensions;
using CryptoMonitoring.Common.Interfaces;
using CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;
using CryptoMonitoring.DataGenerator.Business.Interfaces.RabbitMQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CryptoMonitoring.DataGenerator.Business.Commands.ExternalApis.CoinCapApi;

public class GetCryptoCurrenciesCoinCapCommand : ICommand<object?, IActionResult>
{
    private readonly ICoinCapApiService _coinCapApiService;
    private readonly IRabbitMQPublisherService _rabbitMQPublisherService;
    private readonly string _routingKey;

    public GetCryptoCurrenciesCoinCapCommand(
        ICoinCapApiService coinCapApiService, 
        IRabbitMQPublisherService rabbitMQPublisherService,
        IConfiguration configuration)
    {
        _coinCapApiService = coinCapApiService;
        _rabbitMQPublisherService = rabbitMQPublisherService;
        _routingKey = configuration["RABBITMQ_COINCAP_CRYPTOCURRENCY_PROCESSOR_QUEUE_ROUTING_KEY"]!;
    }

    public async Task<IActionResult> ExecuteAsync(object? request = null)
    {
        var response = await _coinCapApiService.GetCryptoCurrencyAsync();
        await _rabbitMQPublisherService.PublishAsync(response, _routingKey);

        return new object().ToHttpResponse("Crypto currency data from CoinCap Api fetched successfully", 200);
    }
}
