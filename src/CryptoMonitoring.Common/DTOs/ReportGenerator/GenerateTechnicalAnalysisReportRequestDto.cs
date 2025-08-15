namespace CryptoMonitoring.Common.DTOs.ReportGenerator;

public record GenerateTechnicalAnalysisReportRequestDto(
    string Name,
    string Symbol,
    int Period = 30);