using CryptoMonitoring.NotificationService.Business.Commands;
using CryptoMonitoring.NotificationService.Business.Interfaces;
using CryptoMonitoring.NotificationService.Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoMonitoring.NotificationService.Business;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUsersService, UsersService>();

        return services;
    }

    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddScoped<RegisterUserCommand>();
        services.AddScoped<LoginUserCommand>();
        services.AddScoped<UpdateRoleCommand>();

        return services;
    }
}
