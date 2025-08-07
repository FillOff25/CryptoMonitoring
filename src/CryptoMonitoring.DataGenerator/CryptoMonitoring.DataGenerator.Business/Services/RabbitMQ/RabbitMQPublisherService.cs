using CryptoMonitoring.Common.Interfaces.RabbitMQ;
using CryptoMonitoring.DataGenerator.Business.Interfaces.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace CryptoMonitoring.DataGenerator.Business.Services.RabbitMQ;

public class RabbitMQPublisherService : IRabbitMQPublisherService
{
    private readonly IChannel _channel;
    private readonly string _exchangeName;

    public RabbitMQPublisherService(IRabbitMQConnectionFactory factory, IConfiguration configuration)
    {
        _channel = factory.GetChannel();
        _exchangeName = configuration["RABBITMQ_EXCHANGE_NAME"]!;
    }

    public async Task PublishAsync<T>(T data, string routingKey)
    {
        var message = JsonConvert.SerializeObject(data);
        var body = Encoding.UTF8.GetBytes(message);

        await _channel.BasicPublishAsync(_exchangeName, routingKey, body);
    }
}
