using CryptoMonitoring.Common.DTOs.ReportGenerator;
using CryptoMonitoring.Common.Extensions;
using CryptoMonitoring.Common.Interfaces;
using CryptoMonitoring.ReportGenerator.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitoring.ReportGenerator.Business.Commands;

public class GenerateTechnicalAnalysisReportCommand : ICommand<GenerateTechnicalAnalysisReportRequestDto, IActionResult>
{
    private readonly IExcelReportService _excelReportService;

    public GenerateTechnicalAnalysisReportCommand(IExcelReportService excelReportService)
    {
        _excelReportService = excelReportService;
    }

    public async Task<IActionResult> ExecuteAsync(GenerateTechnicalAnalysisReportRequestDto request)
    {
        return (await _excelReportService.GenerateTechnicalAnalysisReportAsync(request))
            .ToHttpResponse("Technical analysis report generated successfully", 201);
    }
}
