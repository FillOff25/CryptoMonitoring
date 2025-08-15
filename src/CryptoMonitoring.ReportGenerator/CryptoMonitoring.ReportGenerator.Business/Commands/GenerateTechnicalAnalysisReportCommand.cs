using CryptoMonitoring.Common.DTOs.ReportGenerator;
using CryptoMonitoring.Common.Interfaces;
using CryptoMonitoring.ReportGenerator.Business.Interfaces;

namespace CryptoMonitoring.ReportGenerator.Business.Commands;

public class GenerateTechnicalAnalysisReportCommand : ICommand<GenerateTechnicalAnalysisReportRequestDto, MemoryStream>
{
    private readonly IExcelReportService _excelReportService;

    public GenerateTechnicalAnalysisReportCommand(IExcelReportService excelReportService)
    {
        _excelReportService = excelReportService;
    }

    public async Task<MemoryStream> ExecuteAsync(GenerateTechnicalAnalysisReportRequestDto request)
    {
        return await _excelReportService.GenerateTechnicalAnalysisReportAsync(request);
    }
}
