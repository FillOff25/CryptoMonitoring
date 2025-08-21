using CryptoMonitoring.Common.DTOs.Auth;
using CryptoMonitoring.Common.Extensions;
using CryptoMonitoring.Common.Interfaces;
using CryptoMonitoring.NotificationService.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitoring.NotificationService.Business.Commands;

public class RegisterUserCommand : ICommand<RegisterUserRequestDto, IActionResult>
{
    private readonly IUsersService _usersService;

    public RegisterUserCommand(IUsersService usersService)
    {
        _usersService = usersService;
    }

    public async Task<IActionResult> ExecuteAsync(RegisterUserRequestDto request)
    {
        await _usersService.RegisterUserAsync(request);

        return (new object()).ToHttpResponse("User registered successfully", 201);
    }
}
