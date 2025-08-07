using CryptoMonitoring.Common.DTOs.CoinGeckoApi;
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

namespace CryptoMonitoring.DataProcessor.Business.Services.RabbitMQ.CoinGecko;

public class RabbitMQCoinGeckoCryptoCurrencyConsumerService : BackgroundService
{
    private readonly IChannel _channel;
    private readonly string _queueName;
    private readonly IServiceScopeFactory _scopeFactory;

    public RabbitMQCoinGeckoCryptoCurrencyConsumerService(
        IRabbitMQConnectionFactory factory,
        IConfiguration configuration,
        IServiceScopeFactory scopeFactory)
    {
        _channel = factory.GetChannel();
        _queueName = configuration["RABBITMQ_COINGECKO_CRYPTOCURRENCY_PROCESSOR_QUEUE_NAME"]!;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Log.Information("Starting CoinGecko cryptocurrency consumer background service");

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                await using var scope = _scopeFactory.CreateAsyncScope();
                var coinGeckoProcessingService = scope.ServiceProvider.GetRequiredService<ICoinGeckoProcessingService>();

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var json = JsonConvert.DeserializeObject<List<CoinGeckoCoinResponseDto>>(message);
                Log.Information("CoinGecko cryptocurrency data received successfully");

                await coinGeckoProcessingService.ProcessCryptoCurrencyAsync(json!);

                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error processing CoinGecko cryptocurrency");
                await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
            }
        };

        await _channel.BasicConsumeAsync(queue: _queueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
    }
}
