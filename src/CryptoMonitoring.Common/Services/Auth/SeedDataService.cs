using CryptoMonitoring.Common.Interfaces.Auth;
using CryptoMonitoring.Common.Persistence.Databases;
using CryptoMonitoring.Common.Persistence.Interfaces;
using CryptoMonitoring.Models.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CryptoMonitoring.Common.Services.Auth;

public class SeedDataService
{
    public static async Task SeedDataAsync(AsyncServiceScope scope, IConfiguration configuration)
    {
        Log.Information("Starting seed database");

        using var context = scope.ServiceProvider.GetRequiredService<CryptoMonitoringDataDbContext>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var user = await unitOfWork.Users.GetByEmailAsync(configuration["ROOT_ADMIN_EMAIL"]!);

        if (user == null)
        {
            Log.Information("Root admin not found. Creating root admin");

            user = new User()
            {
                Id = Guid.NewGuid(),
                Email = configuration["ROOT_ADMIN_EMAIL"]!,
                TelegramId = -1,
                IsEmailNotify = true,
                IsTelegramNotify = false,
                Role = "Admin",
                PasswordHash = HasherService.Generate(configuration["ROOT_ADMIN_PASSWORD"]!)
            };

            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.SaveAsync();

            Log.Information($"Root admin created successfully. Admin Id: {user.Id}");
        }
        else if (!user.IsActive)
        {
            Log.Information("Root admin found, but not active. Activating root admin");
            user.IsActive = true;

            unitOfWork.Users.Update(user);
            await unitOfWork.SaveAsync();

            Log.Information($"Root admin activated successfully. Admin Id: {user.Id}");
        }
        else
        {
            Log.Information($"Root admin found. Admin Id: {user.Id}");
        }
    }
}