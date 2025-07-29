namespace CryptoMonitoring.Common.DTOs.DataGenerator;

public record GenerateDataResponseDto(
    string Name,
    string Symbol,
    int GeneratedCount);