using CryptoMonitoring.Common.DTOs.ReportGenerator;

namespace CryptoMonitoring.ReportGenerator.Business.Interfaces
{
    public interface IExcelReportService
    {
        Task<MemoryStream> GenerateDailyReportAsync(GenerateDailyReportRequestDto dto);
        Task<MemoryStream> GenerateTechnicalAnalysisReportAsync(GenerateTechnicalAnalysisReportRequestDto dto);
    }
}