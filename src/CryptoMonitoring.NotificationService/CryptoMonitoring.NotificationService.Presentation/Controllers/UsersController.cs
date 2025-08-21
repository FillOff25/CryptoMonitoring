using CryptoMonitoring.Common.DTOs.Auth;
using CryptoMonitoring.NotificationService.Business.Commands;
using CryptoMonitoring.NotificationService.Business.Commands.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitoring.NotificationService.Presentation.Controllers;

[ApiController]
[Route("users")]
[Authorize(Policy = "AdminOnly")]
public class UsersController : ControllerBase
{
    private readonly RegisterUserCommand _registerUserCommand;
    private readonly LoginUserCommand _loginUserCommand;
    private readonly UpdateRoleCommand _updateRoleCommand;

    public UsersController(
        RegisterUserCommand registerUserCommand,
        LoginUserCommand loginUserCommand,
        UpdateRoleCommand updateRoleCommand)
    {
        _registerUserCommand = registerUserCommand;
        _loginUserCommand = loginUserCommand;
        _updateRoleCommand = updateRoleCommand;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterUserRequestDto dto)
    {
        return await _registerUserCommand.ExecuteAsync(dto);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginUserAsync([FromBody] LoginUserRequestDto dto)
    {
        return await _loginUserCommand.ExecuteAsync(dto);
    }

    [HttpPost("update-role")]
    public async Task<IActionResult> UpdateRoleAsync([FromBody] UpdateRoleRequestDto dto)
    {
        return await _updateRoleCommand.ExecuteAsync(dto);
    }
}