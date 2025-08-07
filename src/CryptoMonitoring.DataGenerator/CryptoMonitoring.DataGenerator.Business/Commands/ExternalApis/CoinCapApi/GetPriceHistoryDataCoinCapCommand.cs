using CryptoMonitoring.Common.DTOs.CoinCapApi;
using CryptoMonitoring.Common.Extensions;
using CryptoMonitoring.Common.Interfaces;
using CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;
using CryptoMonitoring.DataGenerator.Business.Interfaces.RabbitMQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CryptoMonitoring.DataGenerator.Business.Commands.ExternalApis.CoinCapApi;

public class GetPriceHistoryDataCoinCapCommand : ICommand<CoinCapHistoryDataRequestDto, IActionResult>
{
    private readonly ICoinCapApiService _coinCapApiService;
    private readonly IRabbitMQPublisherService _rabbitMQPublisherService;
    private readonly string _routingKey;

    public GetPriceHistoryDataCoinCapCommand(
        ICoinCapApiService coinCapApiService, 
        IRabbitMQPublisherService rabbitMQPublisherService,
        IConfiguration configuration)
    {
        _coinCapApiService = coinCapApiService;
        _rabbitMQPublisherService = rabbitMQPublisherService;
        _routingKey = configuration["RABBITMQ_COINCAP_PRICEHISTORYDATA_PROCESSOR_QUEUE_ROUTING_KEY"]!;
    }

    public async Task<IActionResult> ExecuteAsync(CoinCapHistoryDataRequestDto request)
    {
        var response = await _coinCapApiService.GetPriceHistoryByIdAsync(request);
        await _rabbitMQPublisherService.PublishAsync(response, _routingKey);

        return new object().ToHttpResponse("Price history data from CoinCap Api fetched successfully", 200);
    }
}
