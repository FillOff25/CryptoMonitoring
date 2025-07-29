using CryptoMonitoring.Common.Extensions;
using CryptoMonitoring.Common.Interfaces;
using CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitoring.DataGenerator.Business.Commands.ExternalApis.CoinCapApi;

public class GetPriceHistoryDataCoinCapCommand : ICommand<object?, IActionResult>
{
    private readonly ICoinCapApiService _coinCapApiService;

    public GetPriceHistoryDataCoinCapCommand(ICoinCapApiService coinCapApiService)
    {
        _coinCapApiService = coinCapApiService;
    }

    public async Task<IActionResult> ExecuteAsync(object? request = null)
    {
        return (await _coinCapApiService.GetCryptoCurrencyAsync())
            .ToHttpResponse("Price history data from CoinCap Api saved successfully", 201);
    }
}
