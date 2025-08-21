using CryptoMonitoring.Common.DTOs.Auth;
using CryptoMonitoring.Common.Extensions;
using CryptoMonitoring.Common.Interfaces;
using CryptoMonitoring.NotificationService.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitoring.NotificationService.Business.Commands;

public class UpdateRoleCommand : ICommand<UpdateRoleRequestDto, IActionResult>
{
    private readonly IUsersService _usersService;

    public UpdateRoleCommand(IUsersService usersService)
    {
        _usersService = usersService;
    }

    public async Task<IActionResult> ExecuteAsync(UpdateRoleRequestDto request)
    {
        await _usersService.UpdateRoleAsync(request);

        return new object().ToHttpResponse("Role updated successfully", 200);
    }
}
