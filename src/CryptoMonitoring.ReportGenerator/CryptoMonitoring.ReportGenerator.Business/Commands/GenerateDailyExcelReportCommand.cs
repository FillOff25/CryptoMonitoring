using CryptoMonitoring.Common.DTOs.ReportGenerator;
using CryptoMonitoring.Common.Extensions;
using CryptoMonitoring.Common.Interfaces;
using CryptoMonitoring.ReportGenerator.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitoring.ReportGenerator.Business.Commands;

public class GenerateDailyExcelReportCommand : ICommand<GenerateDailyReportRequestDto, IActionResult>
{
    private readonly IExcelReportService _excelReportService;

    public GenerateDailyExcelReportCommand(IExcelReportService excelReportService)
    {
        _excelReportService = excelReportService;
    }

    public async Task<IActionResult> ExecuteAsync(GenerateDailyReportRequestDto request)
    {
        return (await _excelReportService.GenerateDailyReportAsync(request))
            .ToHttpResponse("Daily report generated successfully", 201);
    }
}
