using CryptoMonitoring.Common.DTOs.ReportGenerator;
using CryptoMonitoring.Common.Interfaces;
using CryptoMonitoring.ReportGenerator.Business.Interfaces;

namespace CryptoMonitoring.ReportGenerator.Business.Commands;

public class GenerateDailyExcelReportCommand : ICommand<GenerateDailyReportRequestDto, MemoryStream>
{
    private readonly IExcelReportService _excelReportService;

    public GenerateDailyExcelReportCommand(IExcelReportService excelReportService)
    {
        _excelReportService = excelReportService;
    }

    public async Task<MemoryStream> ExecuteAsync(GenerateDailyReportRequestDto request)
    {
        return await _excelReportService.GenerateDailyReportAsync(request);
    }
}
