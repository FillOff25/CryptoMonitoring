using CryptoMonitoring.Common.DTOs.CoinGeckoApi;
using CryptoMonitoring.DataGenerator.Business.Commands.ExternalApis.CoinGeckoApi;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitoring.DataGenerator.Presentation.Controllers;

[ApiController]
[Route("/api/coin-gecko")]
public class CoinGeckoController : ControllerBase
{
    private readonly GetCryptoCurrenciesCoinGeckoCommand _getCryptoCurrenciesCoinGeckoCommand;
    private readonly GetMarketDataCoinGeckoCommand _getMarketDataCoinGeckoCommand;
    private readonly GetPriceHistoryDataCoinGeckoCommand _getPriceHistoryDataCoinGeckoCommand;

    public CoinGeckoController(
        GetCryptoCurrenciesCoinGeckoCommand getCryptoCurrenciesCoinGeckoCommand, 
        GetMarketDataCoinGeckoCommand getMarketDataCoinGeckoCommand, 
        GetPriceHistoryDataCoinGeckoCommand getPriceHistoryDataCoinGeckoCommand)
    {
        _getCryptoCurrenciesCoinGeckoCommand = getCryptoCurrenciesCoinGeckoCommand;
        _getMarketDataCoinGeckoCommand = getMarketDataCoinGeckoCommand;
        _getPriceHistoryDataCoinGeckoCommand = getPriceHistoryDataCoinGeckoCommand;
    }

    [HttpPost("crypto-currency")]
    public async Task<IActionResult> GetCryptoCurrencyAsync()
    {
        return await _getCryptoCurrenciesCoinGeckoCommand.ExecuteAsync();
    }

    [HttpPost("market-data")]
    public async Task<IActionResult> GetMarketDataAsync([FromQuery] CoinGeckoCoinWithMarketDataRequestDto dto)
    {
        return await _getMarketDataCoinGeckoCommand.ExecuteAsync(dto);
    }

    [HttpPost("price-history-data")]
    public async Task<IActionResult> GetPriceHistoryDataAsync([FromQuery] CoinGeckoHistoricalChartDataRequestDto dto)
    {
        return await _getPriceHistoryDataCoinGeckoCommand.ExecuteAsync(dto);
    }
}
