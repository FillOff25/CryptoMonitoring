using CryptoMonitoring.Common.DTOs.DataGenerator;
using CryptoMonitoring.DataGenerator.Business.Commands.DataGenerator;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitoring.DataGenerator.Presentation.Controllers;

[ApiController]
[Route("/api/data-generator")]
public class DataGeneratorController : ControllerBase
{
    private readonly GenerateMarketDataCommand _generateMarketDataCommand;
    private readonly GeneratePriceHistoryDataCommand _generatePriceHistoryDataCommand;

    public DataGeneratorController(
        GenerateMarketDataCommand generateMarketDataCommand, 
        GeneratePriceHistoryDataCommand generatePriceHistoryDataCommand)
    {
        _generateMarketDataCommand = generateMarketDataCommand;
        _generatePriceHistoryDataCommand = generatePriceHistoryDataCommand;
    }

    [HttpPost("market-data")]
    public async Task<IActionResult> GenerateMarketDataAsync([FromQuery] GenerateDataRequestDto dto)
    {
        return await _generateMarketDataCommand.ExecuteAsync(dto);
    }

    [HttpPost("price-history-data")]
    public async Task<IActionResult> GeneratePriceHistoryDataAsync([FromQuery] GenerateDataRequestDto dto)
    {
        return await _generatePriceHistoryDataCommand.ExecuteAsync(dto);
    }
}
