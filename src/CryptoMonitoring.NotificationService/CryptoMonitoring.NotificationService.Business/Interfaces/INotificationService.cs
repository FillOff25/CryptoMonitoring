using CryptoMonitoring.Common.DTOs.NotificationService;

namespace CryptoMonitoring.NotificationService.Business.Interfaces;

public interface INotificationService
{
    Task NotifyUserByTelegramAsync(NotifyUserByTelegramRequestDto dto);
    Task NotifyUserByEmailAsync(NotifyUserByEmailRequestDto dto);
}