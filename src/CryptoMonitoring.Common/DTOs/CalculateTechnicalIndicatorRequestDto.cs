using CryptoMonitoring.Models.Enums;

namespace CryptoMonitoring.Common.DTOs;

public record CalculateTechnicalIndicatorRequestDto(
    string Name,
    string Symbol,
    IndicatorTypeEnum Type);
