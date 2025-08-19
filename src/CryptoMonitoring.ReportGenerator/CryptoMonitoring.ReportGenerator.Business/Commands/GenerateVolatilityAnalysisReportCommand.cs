using CryptoMonitoring.Common.DTOs.ReportGenerator;
using CryptoMonitoring.Common.Extensions;
using CryptoMonitoring.Common.Interfaces;
using CryptoMonitoring.ReportGenerator.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitoring.ReportGenerator.Business.Commands;

public class GenerateVolatilityAnalysisReportCommand : ICommand<GenerateVolatilityAnalysisReportRequestDto, IActionResult>
{
    private readonly IExcelReportService _excelReportService;

    public GenerateVolatilityAnalysisReportCommand(IExcelReportService excelReportService)
    {
        _excelReportService = excelReportService;
    }

    public async Task<IActionResult> ExecuteAsync(GenerateVolatilityAnalysisReportRequestDto request)
    {
        return (await _excelReportService.GenerateVolatilityAnalysisReportAsync(request))
            .ToHttpResponse("Volatility analysis report generated successfully", 201);
    }
}
