using CryptoMonitoring.Common.DTOs.DataGenerator;
using CryptoMonitoring.Common.Interfaces.RabbitMQ;
using CryptoMonitoring.DataProcessor.Business.Interfaces.Processing;
using CryptoMonitoring.Models.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Text;

namespace CryptoMonitoring.DataProcessor.Business.Services.RabbitMQ.SyntheticData;

public class RabbitMQSyntheticPriceHistoryDataConsumerService : BackgroundService
{
    private readonly IChannel _channel;
    private readonly string _queueName;
    private readonly IServiceScopeFactory _scopeFactory;

    public RabbitMQSyntheticPriceHistoryDataConsumerService(
        IRabbitMQConnectionFactory factory,
        IConfiguration configuration,
        IServiceScopeFactory scopeFactory)
    {
        _channel = factory.GetChannel();
        _queueName = configuration["RABBITMQ_SYNTHETIC_PRICEHISTORYDATA_PROCESSOR_QUEUE_NAME"]!;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Log.Information("Starting synthetic price history data consumer background service");

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                await using var scope = _scopeFactory.CreateAsyncScope();
                var syntheticDataProcessingService = scope.ServiceProvider.GetRequiredService<ISyntheticDataProcessingService>();

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var json = JsonConvert.DeserializeObject<GenerateDataRabbitMQResponseDto<List<PriceHistoryData>>>(message);
                Log.Information("Synthetic price history data received successfully");

                await syntheticDataProcessingService.ProcessPriceHistoryDataAsync(json!);

                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error processing synthetic price history data");
                await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
            }
        };

        await _channel.BasicConsumeAsync(queue: _queueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
    }
}
