using CryptoMonitoring.Common.DTOs.CoinCapApi;
using CryptoMonitoring.Common.Interfaces.RabbitMQ;
using CryptoMonitoring.DataProcessor.Business.Interfaces.Processing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Text;

namespace CryptoMonitoring.DataProcessor.Business.Services.RabbitMQ.CoinCap;

public class RabbitMQCoinCapPriceHistoryDataConsumerService : BackgroundService
{
    private readonly IChannel _channel;
    private readonly string _queueName;
    private readonly IServiceScopeFactory _scopeFactory;

    public RabbitMQCoinCapPriceHistoryDataConsumerService(
        IRabbitMQConnectionFactory factory,
        IConfiguration configuration,
        IServiceScopeFactory scopeFactory)
    {
        _channel = factory.GetChannel();
        _queueName = configuration["RABBITMQ_COINCAP_PRICEHISTORYDATA_PROCESSOR_QUEUE_NAME"]!;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Log.Information("Starting CoinCap price history data consumer background service");

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                await using var scope = _scopeFactory.CreateAsyncScope();
                var coinCapProcessingService = scope.ServiceProvider.GetRequiredService<ICoinCapProcessingService>();

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var json = JsonConvert.DeserializeObject<CoinCapPriceHistoryDataRabbitMQResponseDto>(message);
                Log.Information("CoinCap price history data received successfully");

                await coinCapProcessingService.ProcessPriceHistoryByIdAsync(json!);

                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error processing CoinCap price history data");
                await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
            }
        };

        await _channel.BasicConsumeAsync(queue: _queueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
    }
}
