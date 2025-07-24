using CryptoMonitoring.DataGenerator.Business.DTOs.CoinGeckoApi;
using CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitoring.DataGenerator.Presentation.Controllers;

[ApiController]
[Route("/api/coin-gecko")]
public class CoinGeckoController : ControllerBase
{
    private readonly ICoinGeckoApiService _coinGeckoApiService;

    public CoinGeckoController(ICoinGeckoApiService coinGeckoApiService)
    {
        _coinGeckoApiService = coinGeckoApiService;
    }

    [HttpPost("crypto-currency")]
    public async Task<IActionResult> GetCryptoCurrencyAsync()
    {
        await _coinGeckoApiService.GetCryptoCurrencyAsync();

        return Ok();
    }

    [HttpPost("market-data")]
    public async Task<IActionResult> GetMarketDataAsync([FromQuery] CoinGeckoCoinWithMarketDataRequestDto dto)
    {
        await _coinGeckoApiService.GetMarketDataByIdAsync(dto);

        return Ok();
    }

    [HttpPost("price-history-data")]
    public async Task<IActionResult> GetPriceHistoryDataAsync([FromQuery] CoinGeckoHistoricalChartDataRequestDto dto)
    {
        await _coinGeckoApiService.GetPriceHistoryDataByIdAsync(dto);

        return Ok();
    }
}
