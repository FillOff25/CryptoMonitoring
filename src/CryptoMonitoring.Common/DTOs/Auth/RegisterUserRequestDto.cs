namespace CryptoMonitoring.Common.DTOs.Auth;

public record RegisterUserRequestDto(
    string Email,
    string Password,
    bool IsEmailNotify,
    bool IsTelegramNotify,
    long TelegramId = -1);