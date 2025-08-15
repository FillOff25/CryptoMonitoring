using CryptoMonitoring.Models.Enums;

namespace CryptoMonitoring.Common.DTOs.DataProcessor;

public record CalculateTechnicalIndicatorRequestDto(
    string Name,
    string Symbol,
    IndicatorTypeEnum Type);
