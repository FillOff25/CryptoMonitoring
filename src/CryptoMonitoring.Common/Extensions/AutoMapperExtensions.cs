using CryptoMonitoring.Common.Mapper.Profiles;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoMonitoring.Common.Extensions;

public static class AutoMapperExtensions
{
    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, typeof(CryptoCurrencyProfile).Assembly);

        return services;
    }
}
