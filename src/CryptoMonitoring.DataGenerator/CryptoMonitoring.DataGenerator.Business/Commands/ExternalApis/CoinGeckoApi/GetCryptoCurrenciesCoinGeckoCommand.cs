using CryptoMonitoring.Common.Extensions;
using CryptoMonitoring.Common.Interfaces;
using CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitoring.DataGenerator.Business.Commands.ExternalApis.CoinGeckoApi;

public class GetCryptoCurrenciesCoinGeckoCommand : ICommand<object?, IActionResult>
{
    private readonly ICoinGeckoApiService _coinGeckoApiService;

    public GetCryptoCurrenciesCoinGeckoCommand(ICoinGeckoApiService coinGeckoApiService)
    {
        _coinGeckoApiService = coinGeckoApiService;
    }

    public async Task<IActionResult> ExecuteAsync(object? request = null)
    {
        return (await _coinGeckoApiService.GetCryptoCurrencyAsync())
            .ToHttpResponse("Crypto currency data from CoinGecko Api saved successfully", 201);
    }
}
