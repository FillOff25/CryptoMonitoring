using CryptoMonitoring.Common.Interfaces.RabbitMQ;
using CryptoMonitoring.Common.Services.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoMonitoring.Common.Extensions;

public static class RabbitMQExtensions
{
    public static IServiceCollection AddRabbitMQConnectionFactory(this IServiceCollection services)
    {
        services.AddSingleton<IRabbitMQConnectionFactory, RabbitMQConnectionFactory>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var factory = new RabbitMQConnectionFactory(configuration);

            factory.InitializeAsync().Wait();
            factory.DeclareTopologyAsync().Wait();

            return factory;
        });

        return services;
    }
}
