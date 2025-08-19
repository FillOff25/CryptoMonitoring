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
    private readonly GenerateComparativeAnalysisReportCommand _generateComparativeAnalysisReportCommand;
    private readonly GenerateVolatilityAnalysisReportCommand _generateVolatilityAnalysisReportCommand;
    private readonly DownloadReportCommand _downloadReportCommand;

    public ExcelReportController(
        GenerateDailyExcelReportCommand generateDailyExcelReportCommand,
        GenerateTechnicalAnalysisReportCommand generateeTechnicalAnalysisReportCommand,
        GenerateComparativeAnalysisReportCommand generateComparativeAnalysisReportCommand,
        GenerateVolatilityAnalysisReportCommand generateVolatilityAnalysisReportCommand,
        DownloadReportCommand downloadReportCommand)
    {
        _generateDailyExcelReportCommand = generateDailyExcelReportCommand;
        _generateTechnicalAnalysisReportCommand = generateeTechnicalAnalysisReportCommand;
        _generateComparativeAnalysisReportCommand = generateComparativeAnalysisReportCommand;
        _generateVolatilityAnalysisReportCommand = generateVolatilityAnalysisReportCommand;
        _downloadReportCommand = downloadReportCommand;
    }

    [HttpPost("daily")]
    public async Task<IActionResult> GenerateDailyReportAsync([FromQuery] GenerateDailyReportRequestDto dto)
    {
        return await _generateDailyExcelReportCommand.ExecuteAsync(dto);
    }

    [HttpPost("technical-analisis")]
    public async Task<IActionResult> GenerateTechnicalAnalysisReportAsync([FromQuery] GenerateTechnicalAnalysisReportRequestDto dto)
    {
        return await _generateTechnicalAnalysisReportCommand.ExecuteAsync(dto);
    }

    [HttpPost("comparative-analisis")]
    public async Task<IActionResult> GenerateTechnicalAnalysisReportAsync([FromBody] GenerateComparativeAnalysisReportRequestDto dto)
    {
        return await _generateComparativeAnalysisReportCommand.ExecuteAsync(dto);
    }

    [HttpPost("volatility-analisis")]
    public async Task<IActionResult> GenerateVolatilityAnalysisReportAsync([FromBody] GenerateVolatilityAnalysisReportRequestDto dto)
    {
        return await _generateVolatilityAnalysisReportCommand.ExecuteAsync(dto);
    }

    [HttpGet("download")]
    public async Task<IActionResult> DownloadReportAsync([FromQuery] DownloadReportRequestDto dto)
    {
        var response = await _downloadReportCommand.ExecuteAsync(dto);
        return File(response.Stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", response.OriginalFileName);
    }
}