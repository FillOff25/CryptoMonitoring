namespace CryptoMonitoring.Common.DTOs.Auth;

public record LoginUserRequestDto(
    string Email,
    string Password);