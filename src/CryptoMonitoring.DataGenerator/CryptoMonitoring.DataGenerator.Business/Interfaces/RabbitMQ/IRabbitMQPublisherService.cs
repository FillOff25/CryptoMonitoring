namespace CryptoMonitoring.DataGenerator.Business.Interfaces.RabbitMQ
{
    public interface IRabbitMQPublisherService
    {
        Task PublishAsync<T>(T data, string routingKey);
    }
}