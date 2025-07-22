using CryptoMonitoring.DataGenerator.Business.DTOs.CoinCapApi;
using CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitoring.DataGenerator.Presentation.Controllers;

[ApiController]
[Route("/api/coincap")]
public class CoinCapController : ControllerBase
{
    private readonly ICoinCapApiService _coinCapApiService;

    public CoinCapController(ICoinCapApiService coinCapApiService)
    {
        _coinCapApiService = coinCapApiService;
    }

    [HttpPost("crypto-currency")]
    public async Task<IActionResult> GetCryptoCurrencies()
    {
        await _coinCapApiService.GetCryptoCurrencyAsync();

        return Ok();
    }

    [HttpPost("market-data")]
    public async Task<IActionResult> GetMarketData()
    {
        await _coinCapApiService.GetMarketDataAsync();

        return Ok();
    }

    [HttpPost("price-history-data")]
    public async Task<IActionResult> GetPriceHistoryData([FromQuery] CoinCapHistoryDataRequestDto dto)
    {
        await _coinCapApiService.GetPriceHistoryAsync(dto);

        return Ok();
    }
}
