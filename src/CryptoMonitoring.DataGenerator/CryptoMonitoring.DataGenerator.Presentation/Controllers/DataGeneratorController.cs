using CryptoMonitoring.DataGenerator.Business.DTOs.DataGenerator;
using CryptoMonitoring.DataGenerator.Business.Interfaces.DataGenerator;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitoring.DataGenerator.Presentation.Controllers;

[ApiController]
[Route("/api/data-generator")]
public class DataGeneratorController : ControllerBase
{
    private readonly IDataGeneratorService _dataGeneratorService;

    public DataGeneratorController(IDataGeneratorService dataGeneratorService)
    {
        _dataGeneratorService = dataGeneratorService;
    }

    [HttpPost("market-data")]
    public async Task<IActionResult> GenerateMarketDataAsync([FromQuery] GenerateDataRequestDto dto)
    {
        await _dataGeneratorService.GenerateMarketDataAsync(dto);

        return Ok();
    }

    [HttpPost("price-history-data")]
    public async Task<IActionResult> GeneratePriceHistoryDataAsync([FromQuery] GenerateDataRequestDto dto)
    {
        await _dataGeneratorService.GeneratePriceHistoryDataAsync(dto);

        return Ok();
    }
}
