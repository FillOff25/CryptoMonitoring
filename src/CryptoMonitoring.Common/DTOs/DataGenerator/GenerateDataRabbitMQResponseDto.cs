namespace CryptoMonitoring.Common.DTOs.DataGenerator;

public record GenerateDataRabbitMQResponseDto<T>(
    string Name,
    string Symbol,
    T Data);