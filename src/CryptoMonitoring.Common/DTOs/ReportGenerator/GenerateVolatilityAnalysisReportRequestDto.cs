namespace CryptoMonitoring.Common.DTOs.ReportGenerator;

public record GenerateVolatilityAnalysisReportRequestDto(
    string Name,
    string Symbol,
    int Period = 30);