using CryptoMonitoring.Common.DTOs.CoinCapApi;
using CryptoMonitoring.DataGenerator.Business.Commands.ExternalApis.CoinCapApi;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitoring.DataGenerator.Presentation.Controllers;

[ApiController]
[Route("/api/coin-cap")]
public class CoinCapController : ControllerBase
{
    private readonly GetCryptoCurrenciesCoinCapCommand _getCryptoCurrenciesCoinCapCommand;
    private readonly GetMarketDataCoinCapCommand _getMarketDataCoinCapCommand;
    private readonly GetPriceHistoryDataCoinCapCommand _getPriceHistoryDataCoinCapCommand;

    public CoinCapController(
        GetCryptoCurrenciesCoinCapCommand getCryptoCurrenciesCoinCapCommand,
        GetMarketDataCoinCapCommand getMarketDataCoinCapCommand,
        GetPriceHistoryDataCoinCapCommand getPriceHistoryDataCoinCapCommand)
    {
        _getCryptoCurrenciesCoinCapCommand = getCryptoCurrenciesCoinCapCommand;
        _getMarketDataCoinCapCommand = getMarketDataCoinCapCommand;
        _getPriceHistoryDataCoinCapCommand = getPriceHistoryDataCoinCapCommand;
    }

    [HttpPost("crypto-currency")]
    public async Task<IActionResult> GetCryptoCurrenciesAsync()
    {
        return await _getCryptoCurrenciesCoinCapCommand.ExecuteAsync();
    }

    [HttpPost("market-data")]
    public async Task<IActionResult> GetMarketDataAsync()
    {
        return await _getMarketDataCoinCapCommand.ExecuteAsync();
    }

    [HttpPost("price-history-data")]
    public async Task<IActionResult> GetPriceHistoryDataAsync([FromQuery] CoinCapHistoryDataRequestDto dto)
    {
        return await _getPriceHistoryDataCoinCapCommand.ExecuteAsync(dto);
    }
}
