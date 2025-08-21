using CryptoMonitoring.Common.DTOs.Auth;

namespace CryptoMonitoring.NotificationService.Business.Interfaces;

public interface IUsersService
{
    Task<string> LoginUserAsync(LoginUserRequestDto dto);
    Task RegisterUserAsync(RegisterUserRequestDto dto);
    Task UpdateRoleAsync(UpdateRoleRequestDto dto);
}