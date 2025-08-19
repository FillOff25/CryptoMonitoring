using CryptoMonitoring.Models.Enums;

namespace CryptoMonitoring.Common.DTOs.ReportGenerator;

public record GenerateReportResponseDto(
    Guid Id,
    string OriginalFileName,
    ExcelReportTypeEnum ReportType,
    DateTime CreatedAt);