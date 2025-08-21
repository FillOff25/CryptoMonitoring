using CryptoMonitoring.Common.DTOs.NotificationService;
using CryptoMonitoring.NotificationService.Business.Commands.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitoring.NotificationService.Presentation.Controllers;

[ApiController]
[Route("notification")]
[Authorize(Policy = "UserAndAdmin")]
public class NotificationController : ControllerBase
{
    private readonly NotifyUserByEmailCommand _notifyUserByEmailCommand;
    private readonly NotifyUserByTelegramCommand _notifyUserByTelegramCommand;

    public NotificationController(
        NotifyUserByEmailCommand notifyUserByEmailCommand, 
        NotifyUserByTelegramCommand notifyUserByTelegramCommand)
    {
        _notifyUserByEmailCommand = notifyUserByEmailCommand;
        _notifyUserByTelegramCommand = notifyUserByTelegramCommand;
    }

    [HttpGet("email")]
    public async Task<IActionResult> NotifyUserByEmailAsync([FromQuery] NotifyUserByEmailRequestDto dto)
    {
        return await _notifyUserByEmailCommand.ExecuteAsync(dto);
    }

    [HttpGet("telegram")]
    public async Task<IActionResult> NotifyUserByTelegramAsync([FromQuery] NotifyUserByTelegramRequestDto dto)
    {
        return await _notifyUserByTelegramCommand.ExecuteAsync(dto);
    }
}
