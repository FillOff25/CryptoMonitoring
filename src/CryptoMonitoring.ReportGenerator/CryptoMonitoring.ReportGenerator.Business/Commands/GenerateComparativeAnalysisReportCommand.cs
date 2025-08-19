using CryptoMonitoring.Common.DTOs.ReportGenerator;
using CryptoMonitoring.Common.Extensions;
using CryptoMonitoring.Common.Interfaces;
using CryptoMonitoring.ReportGenerator.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitoring.ReportGenerator.Business.Commands;

public class GenerateComparativeAnalysisReportCommand : ICommand<GenerateComparativeAnalysisReportRequestDto, IActionResult>
{
    private readonly IExcelReportService _excelReportService;

    public GenerateComparativeAnalysisReportCommand(IExcelReportService excelReportService)
    {
        _excelReportService = excelReportService;
    }

    public async Task<IActionResult> ExecuteAsync(GenerateComparativeAnalysisReportRequestDto request)
    {
        return (await _excelReportService.GenerateComparativeAnalysisReportAsync(request))
            .ToHttpResponse("Comparative analysis report generated successfully", 201);
    }
}