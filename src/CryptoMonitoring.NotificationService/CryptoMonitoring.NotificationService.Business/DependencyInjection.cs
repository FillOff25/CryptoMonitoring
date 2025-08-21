using CryptoMonitoring.NotificationService.Business.Commands;
using CryptoMonitoring.NotificationService.Business.Commands.Auth;
using CryptoMonitoring.NotificationService.Business.Commands.Notifications;
using CryptoMonitoring.NotificationService.Business.Interfaces;
using CryptoMonitoring.NotificationService.Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoMonitoring.NotificationService.Business;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<INotificationService, Services.NotificationService>();

        return services;
    }

    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddScoped<RegisterUserCommand>();
        services.AddScoped<LoginUserCommand>();
        services.AddScoped<UpdateRoleCommand>();

        services.AddScoped<NotifyUserByEmailCommand>();
        services.AddScoped<NotifyUserByTelegramCommand>();

        return services;
    }
}
