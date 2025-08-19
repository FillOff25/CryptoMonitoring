namespace CryptoMonitoring.Common.DTOs.ReportGenerator;

public record DownloadReportResponseDto(
    FileStream Stream,
    string OriginalFileName);