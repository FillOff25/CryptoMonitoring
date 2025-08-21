using CryptoMonitoring.Common.DTOs.NotificationService;
using CryptoMonitoring.Common.Extensions;
using CryptoMonitoring.Common.Interfaces;
using CryptoMonitoring.NotificationService.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitoring.NotificationService.Business.Commands.Notifications;

public class NotifyUserByEmailCommand : ICommand<NotifyUserByEmailRequestDto, IActionResult>
{
    private readonly INotificationService _notificationService;

    public NotifyUserByEmailCommand(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task<IActionResult> ExecuteAsync(NotifyUserByEmailRequestDto request)
    {
        await _notificationService.NotifyUserByEmailAsync(request);

        return new object().ToHttpResponse("User notified by email successfully", 200);
    }
}
