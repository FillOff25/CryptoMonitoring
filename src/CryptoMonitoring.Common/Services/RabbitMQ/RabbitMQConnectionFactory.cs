using CryptoMonitoring.Common.Interfaces.RabbitMQ;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Serilog;

namespace CryptoMonitoring.Common.Services.RabbitMQ;

public class RabbitMQConnectionFactory : IRabbitMQConnectionFactory
{
    private readonly ConnectionFactory _factory;
    private IConnection? _connection;
    private IChannel? _channel;

    private readonly string _exchangeName;
    private readonly string _coinCapCryptoCurrencyQueueName;
    private readonly string _coinGeckoCryptoCurrencyQueueName;
    private readonly string _coinCapMarketDataQueueName;
    private readonly string _coinGeckoMarketDataQueueName;
    private readonly string _coinCapPriceHistoryQueueName;
    private readonly string _coinGeckoPriceHistoryQueueName;
    private readonly string _syntheticMarketDataQueueName;
    private readonly string _syntheticPriceHistoryQueueName;

    private readonly string _coinCapCryptoCurrencyQueueRoutingKey;
    private readonly string _coinGeckoCryptoCurrencyQueueRoutingKey;
    private readonly string _coinCapMarketDataQueueRoutingKey;
    private readonly string _coinGeckoMarketDataQueueRoutingKey;
    private readonly string _coinCapPriceHistoryQueueRoutingKey;
    private readonly string _coinGeckoPriceHistoryQueueRoutingKey;
    private readonly string _syntheticMarketDataQueueRoutingKey;
    private readonly string _syntheticPriceHistoryQueueRoutingKey;

    public RabbitMQConnectionFactory(IConfiguration configuration)
    {
        _factory = new ConnectionFactory()
        {
            HostName = configuration["RABBITMQ_HOSTNAME"]!,
            Port = int.Parse(configuration["RABBITMQ_PORT"]!),
            UserName = configuration["RABBITMQ_DEFAULT_USER"]!,
            Password = configuration["RABBITMQ_DEFAULT_PASS"]!
        };

        _exchangeName = configuration["RABBITMQ_EXCHANGE_NAME"]!;

        _coinCapCryptoCurrencyQueueName = configuration["RABBITMQ_COINCAP_CRYPTOCURRENCY_PROCESSOR_QUEUE_NAME"]!;
        _coinGeckoCryptoCurrencyQueueName = configuration["RABBITMQ_COINGECKO_CRYPTOCURRENCY_PROCESSOR_QUEUE_NAME"]!;
        _coinCapMarketDataQueueName = configuration["RABBITMQ_COINCAP_MARKETDATA_PROCESSOR_QUEUE_NAME"]!;
        _coinGeckoMarketDataQueueName = configuration["RABBITMQ_COINGECKO_MARKETDATA_PROCESSOR_QUEUE_NAME"]!;
        _coinCapPriceHistoryQueueName = configuration["RABBITMQ_COINCAP_PRICEHISTORYDATA_PROCESSOR_QUEUE_NAME"]!;
        _coinGeckoPriceHistoryQueueName = configuration["RABBITMQ_COINGECKO_PRICEHISTORYDATA_PROCESSOR_QUEUE_NAME"]!;
        _syntheticMarketDataQueueName = configuration["RABBITMQ_SYNTHETIC_MARKETDATA_PROCESSOR_QUEUE_NAME"]!;
        _syntheticPriceHistoryQueueName = configuration["RABBITMQ_SYNTHETIC_PRICEHISTORYDATA_PROCESSOR_QUEUE_NAME"]!;

        _coinCapCryptoCurrencyQueueRoutingKey = configuration["RABBITMQ_COINCAP_CRYPTOCURRENCY_PROCESSOR_QUEUE_ROUTING_KEY"]!;
        _coinGeckoCryptoCurrencyQueueRoutingKey = configuration["RABBITMQ_COINGECKO_CRYPTOCURRENCY_PROCESSOR_QUEUE_ROUTING_KEY"]!;
        _coinCapMarketDataQueueRoutingKey = configuration["RABBITMQ_COINCAP_MARKETDATA_PROCESSOR_QUEUE_ROUTING_KEY"]!;
        _coinGeckoMarketDataQueueRoutingKey = configuration["RABBITMQ_COINGECKO_MARKETDATA_PROCESSOR_QUEUE_ROUTING_KEY"]!;
        _coinCapPriceHistoryQueueRoutingKey = configuration["RABBITMQ_COINCAP_PRICEHISTORYDATA_PROCESSOR_QUEUE_ROUTING_KEY"]!;
        _coinGeckoPriceHistoryQueueRoutingKey = configuration["RABBITMQ_COINGECKO_PRICEHISTORYDATA_PROCESSOR_QUEUE_ROUTING_KEY"]!;
        _syntheticMarketDataQueueRoutingKey = configuration["RABBITMQ_SYNTHETIC_MARKETDATA_PROCESSOR_QUEUE_ROUTING_KEY"]!;
        _syntheticPriceHistoryQueueRoutingKey = configuration["RABBITMQ_SYNTHETIC_PRICEHISTORYDATA_PROCESSOR_QUEUE_ROUTING_KEY"]!;
    }

    public async Task InitializeAsync()
    {
        try
        {
            _connection = await _factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            Log.Information("Channel and connection initialized successfully");
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error connecting to RabbitMQ");
            throw;
        }
    }

    public async Task DeclareTopologyAsync()
    {
        if (_channel == null)
        {
            throw new InvalidOperationException("RabbitMQ channel is not initialized. Use InitializeAsync()");
        }

        await _channel.ExchangeDeclareAsync(_exchangeName, ExchangeType.Direct);

        await _channel.QueueDeclareAsync(queue: _coinCapCryptoCurrencyQueueName, autoDelete: false, exclusive: false);
        await _channel.QueueDeclareAsync(queue: _coinGeckoCryptoCurrencyQueueName, autoDelete: false, exclusive: false);
        await _channel.QueueDeclareAsync(queue: _coinCapMarketDataQueueName, autoDelete: false, exclusive: false);
        await _channel.QueueDeclareAsync(queue: _coinGeckoMarketDataQueueName, autoDelete: false, exclusive: false);
        await _channel.QueueDeclareAsync(queue: _coinCapPriceHistoryQueueName, autoDelete: false, exclusive: false);
        await _channel.QueueDeclareAsync(queue: _coinGeckoPriceHistoryQueueName, autoDelete: false, exclusive: false);
        await _channel.QueueDeclareAsync(queue: _syntheticMarketDataQueueName, autoDelete: false, exclusive: false);
        await _channel.QueueDeclareAsync(queue: _syntheticPriceHistoryQueueName, autoDelete: false, exclusive: false);

        await _channel.QueueBindAsync(queue: _coinCapCryptoCurrencyQueueName, exchange: _exchangeName, routingKey: _coinCapCryptoCurrencyQueueRoutingKey);
        await _channel.QueueBindAsync(queue: _coinGeckoCryptoCurrencyQueueName, exchange: _exchangeName, routingKey: _coinGeckoCryptoCurrencyQueueRoutingKey);
        await _channel.QueueBindAsync(queue: _coinCapMarketDataQueueName, exchange: _exchangeName, routingKey: _coinCapMarketDataQueueRoutingKey);
        await _channel.QueueBindAsync(queue: _coinGeckoMarketDataQueueName, exchange: _exchangeName, routingKey: _coinGeckoMarketDataQueueRoutingKey);
        await _channel.QueueBindAsync(queue: _coinCapPriceHistoryQueueName, exchange: _exchangeName, routingKey: _coinCapPriceHistoryQueueRoutingKey);
        await _channel.QueueBindAsync(queue: _coinGeckoPriceHistoryQueueName, exchange: _exchangeName, routingKey: _coinGeckoPriceHistoryQueueRoutingKey);
        await _channel.QueueBindAsync(queue: _syntheticMarketDataQueueName, exchange: _exchangeName, routingKey: _syntheticMarketDataQueueRoutingKey);
        await _channel.QueueBindAsync(queue: _syntheticPriceHistoryQueueName, exchange: _exchangeName, routingKey: _syntheticPriceHistoryQueueRoutingKey);

        Log.Information("Topology declared successfully");
    }

    public IChannel GetChannel()
    {
        if (_channel == null || !_channel.IsOpen)
        {
            throw new InvalidOperationException("RabbitMQ channel is not available");
        }

        return _channel;
    }
}