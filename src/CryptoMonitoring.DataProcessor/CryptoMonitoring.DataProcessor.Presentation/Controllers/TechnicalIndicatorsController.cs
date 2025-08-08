using CryptoMonitoring.Common.DTOs;
using CryptoMonitoring.DataProcessor.Business.Commands.TechnicalIndicators;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitoring.DataProcessor.Presentation.Controllers;

[ApiController]
[Route("/api/technical-indicators")]
public class TechnicalIndicatorsController : ControllerBase
{
    private readonly CalculateTechnicalIndicatorCommand _calculateTechnicalIndicatorCommand;

    public TechnicalIndicatorsController(CalculateTechnicalIndicatorCommand calculateTechnicalIndicatorCommand)
    {
        _calculateTechnicalIndicatorCommand = calculateTechnicalIndicatorCommand;
    }

    [HttpPost]
    public async Task<IActionResult> CalculateTechnicalIndicatorAsync([FromQuery] CalculateTechnicalIndicatorRequestDto dto)
    {
        return await _calculateTechnicalIndicatorCommand.ExecuteAsync(dto);
    }
}