using CryptoMonitoring.Common.DTOs.ReportGenerator;

namespace CryptoMonitoring.ReportGenerator.Business.Interfaces
{
    public interface IExcelReportService
    {
        Task<GenerateReportResponseDto> GenerateDailyReportAsync(GenerateDailyReportRequestDto dto);
        Task<GenerateReportResponseDto> GenerateTechnicalAnalysisReportAsync(GenerateTechnicalAnalysisReportRequestDto dto);
        Task<GenerateReportResponseDto> GenerateComparativeAnalysisReportAsync(GenerateComparativeAnalysisReportRequestDto dto);
        Task<GenerateReportResponseDto> GenerateVolatilityAnalysisReportAsync(GenerateVolatilityAnalysisReportRequestDto dto);
        Task<DownloadReportResponseDto> DownloadReportAsync(DownloadReportRequestDto dto);
    }
}