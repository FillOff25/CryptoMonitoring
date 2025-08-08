using CryptoMonitoring.Common.DTOs;
using CryptoMonitoring.Common.Extensions;
using CryptoMonitoring.Common.Interfaces;
using CryptoMonitoring.DataProcessor.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitoring.DataProcessor.Business.Commands.TechnicalIndicators;

public class CalculateTechnicalIndicatorCommand : ICommand<CalculateTechnicalIndicatorRequestDto, IActionResult>
{
    private readonly ITechnicalIndicatorsService _technicalIndicatorsService;

    public CalculateTechnicalIndicatorCommand(ITechnicalIndicatorsService technicalIndicatorsService)
    {
        _technicalIndicatorsService = technicalIndicatorsService;
    }

    public async Task<IActionResult> ExecuteAsync(CalculateTechnicalIndicatorRequestDto request)
    {
        await _technicalIndicatorsService.CalculateTechnicalIndicatorAsync(request);

        return new object().ToHttpResponse("Technical indicator calculated successfully", 201);
    }
}
