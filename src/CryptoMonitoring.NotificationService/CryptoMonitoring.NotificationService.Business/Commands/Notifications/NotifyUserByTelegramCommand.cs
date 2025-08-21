using CryptoMonitoring.Common.DTOs.NotificationService;
using CryptoMonitoring.Common.Extensions;
using CryptoMonitoring.Common.Interfaces;
using CryptoMonitoring.NotificationService.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitoring.NotificationService.Business.Commands.Notifications;

public class NotifyUserByTelegramCommand : ICommand<NotifyUserByTelegramRequestDto, IActionResult>
{
    private readonly INotificationService _notificationService;

    public NotifyUserByTelegramCommand(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task<IActionResult> ExecuteAsync(NotifyUserByTelegramRequestDto request)
    {
        await _notificationService.NotifyUserByTelegramAsync(request);

        return new object().ToHttpResponse("User notified by telegram successfully", 200);
    }
}
