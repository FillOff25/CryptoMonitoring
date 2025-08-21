namespace CryptoMonitoring.Common.DTOs.Auth;

public record UpdateRoleRequestDto(
    string Email,
    string Role);