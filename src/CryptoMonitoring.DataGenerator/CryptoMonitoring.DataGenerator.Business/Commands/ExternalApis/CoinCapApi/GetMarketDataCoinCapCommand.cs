using CryptoMonitoring.Common.Extensions;
using CryptoMonitoring.Common.Interfaces;
using CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitoring.DataGenerator.Business.Commands.ExternalApis.CoinCapApi;

public class GetMarketDataCoinCapCommand : ICommand<object?, IActionResult>
{
    private readonly ICoinCapApiService _coinCapApiService;

    public GetMarketDataCoinCapCommand(ICoinCapApiService coinCapApiService)
    {
        _coinCapApiService = coinCapApiService;
    }

    public async Task<IActionResult> ExecuteAsync(object? request = null)
    {
        return (await _coinCapApiService.GetMarketDataAsync())
            .ToHttpResponse("Market data saved from CoinCap Api successfully", 201);
    }
}
