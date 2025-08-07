using CryptoMonitoring.Common.DTOs.DataGenerator;
using CryptoMonitoring.Common.Extensions;
using CryptoMonitoring.Common.Interfaces;
using CryptoMonitoring.DataGenerator.Business.Interfaces.DataGenerator;
using CryptoMonitoring.DataGenerator.Business.Interfaces.RabbitMQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CryptoMonitoring.DataGenerator.Business.Commands.DataGenerator;

public class GenerateMarketDataCommand : ICommand<GenerateDataRequestDto, IActionResult>
{
    private readonly IDataGeneratorService _dataGeneratorService;
    private readonly IRabbitMQPublisherService _rabbitMQPublisherService;
    private readonly string _routingKey;

    public GenerateMarketDataCommand(
        IDataGeneratorService dataGeneratorService, 
        IRabbitMQPublisherService rabbitMQPublisherService,
        IConfiguration configuration)
    {
        _dataGeneratorService = dataGeneratorService;
        _rabbitMQPublisherService = rabbitMQPublisherService;
        _routingKey = configuration["RABBITMQ_SYNTHETIC_MARKETDATA_PROCESSOR_QUEUE_ROUTING_KEY"]!;
    }

    public async Task<IActionResult> ExecuteAsync(GenerateDataRequestDto dto)
    {
        var response = await _dataGeneratorService.GenerateMarketDataAsync(dto);
        await _rabbitMQPublisherService.PublishAsync(response, _routingKey);

        return new object().ToHttpResponse("Market data generated successfully", 200);
    }
}
