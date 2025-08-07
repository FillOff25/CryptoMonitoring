using RabbitMQ.Client;

namespace CryptoMonitoring.Common.Interfaces.RabbitMQ
{
    public interface IRabbitMQConnectionFactory
    {
        Task DeclareTopologyAsync();
        IChannel GetChannel();
        Task InitializeAsync();
    }
}