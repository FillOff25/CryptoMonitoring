namespace CryptoMonitoring.Common.DTOs.NotificationService;

public record NotifyUserByEmailRequestDto(
    string Email,
    string Subject,
    string Message);