using CryptoMonitoring.Common.DTOs.Auth;
using CryptoMonitoring.Common.Extensions;
using CryptoMonitoring.Common.Interfaces;
using CryptoMonitoring.NotificationService.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitoring.NotificationService.Business.Commands.Auth;

public class LoginUserCommand : ICommand<LoginUserRequestDto, IActionResult>
{
    private readonly IUsersService _usersService;

    public LoginUserCommand(IUsersService usersService)
    {
        _usersService = usersService;
    }
    public async Task<IActionResult> ExecuteAsync(LoginUserRequestDto request)
    {
        return (await _usersService.LoginUserAsync(request))
            .ToHttpResponse("User logged in successfully", 200);
    }
}
