namespace CryptoMonitoring.Common.DTOs.NotificationService;

public record NotifyUserByTelegramRequestDto(
    long TelegramId,
    string Message);