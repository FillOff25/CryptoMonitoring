using CryptoMonitoring.Common.DTOs.CoinGeckoApi;
using CryptoMonitoring.Common.Extensions;
using CryptoMonitoring.Common.Interfaces;
using CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;
using CryptoMonitoring.DataGenerator.Business.Interfaces.RabbitMQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CryptoMonitoring.DataGenerator.Business.Commands.ExternalApis.CoinGeckoApi;

public class GetPriceHistoryDataCoinGeckoCommand : ICommand<CoinGeckoHistoricalChartDataRequestDto, IActionResult>
{
    private readonly ICoinGeckoApiService _coinGeckoApiService;
    private readonly IRabbitMQPublisherService _rabbitMQPublisherService;
    private readonly string _routingKey;

    public GetPriceHistoryDataCoinGeckoCommand(
        ICoinGeckoApiService coinGeckoApiService, 
        IRabbitMQPublisherService rabbitMQPublisherService,
        IConfiguration configuration)
    {
        _coinGeckoApiService = coinGeckoApiService;
        _rabbitMQPublisherService = rabbitMQPublisherService;
        _routingKey = configuration["RABBITMQ_COINGECKO_PRICEHISTORYDATA_PROCESSOR_QUEUE_ROUTING_KEY"]!;
    }

    public async Task<IActionResult> ExecuteAsync(CoinGeckoHistoricalChartDataRequestDto request)
    {
        var response = await _coinGeckoApiService.GetPriceHistoryDataByIdAsync(request);
        await _rabbitMQPublisherService.PublishAsync(response, _routingKey);

        return new object().ToHttpResponse("Price history data from CoinGecko Api fetched successfully", 200);
    }
}
