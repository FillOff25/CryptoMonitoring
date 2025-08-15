using CryptoMonitoring.Common.DTOs.ReportGenerator;
using CryptoMonitoring.ReportGenerator.Business.Commands;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitoring.ReportGenerator.Presentation.Controllers;

[ApiController]
[Route("/api/excel-report")]
public class ExcelReportController : ControllerBase
{
    private readonly GenerateDailyExcelReportCommand _generateDailyExcelReportCommand;
    private readonly GenerateTechnicalAnalysisReportCommand _generateTechnicalAnalysisReportCommand;

    public ExcelReportController(
        GenerateDailyExcelReportCommand generateDailyExcelReportCommand, 
        GenerateTechnicalAnalysisReportCommand generateeTechnicalAnalysisReportCommand)
    {
        _generateDailyExcelReportCommand = generateDailyExcelReportCommand;
        _generateTechnicalAnalysisReportCommand = generateeTechnicalAnalysisReportCommand;
    }

    [HttpGet("daily")]
    public async Task<IActionResult> GenerateDailyReportAsync([FromQuery] GenerateDailyReportRequestDto dto)
    {
        var stream = await _generateDailyExcelReportCommand.ExecuteAsync(dto);
        var fileName = $"Daily_Report_{dto.Name}_{dto.Symbol}_{DateTime.Now:yyyy-MM-dd}.xlsx";

        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }

    [HttpGet("technical-analisis")]
    public async Task<IActionResult> GenerateTechnicalAnalysisReportAsync([FromQuery] GenerateTechnicalAnalysisReportRequestDto dto)
    {
        var stream = await _generateTechnicalAnalysisReportCommand.ExecuteAsync(dto);
        var fileName = $"Technical_Analisis_Report_{dto.Name}_{dto.Symbol}.xlsx";

        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }
}
