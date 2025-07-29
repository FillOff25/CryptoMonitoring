using CryptoMonitoring.Common.Extensions;
using CryptoMonitoring.Common.Interfaces;
using CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitoring.DataGenerator.Business.Commands.ExternalApis.CoinCapApi;

public class GetCryptoCurrenciesCoinCapCommand : ICommand<object?, IActionResult>
{
    private readonly ICoinCapApiService _coinCapApiService;

    public GetCryptoCurrenciesCoinCapCommand(ICoinCapApiService coinCapApiService)
    {
        _coinCapApiService = coinCapApiService;
    }

    public async Task<IActionResult> ExecuteAsync(object? request = null)
    {
        return (await _coinCapApiService.GetCryptoCurrencyAsync())
            .ToHttpResponse("Crypto currency data from CoinCap Api saved successfully", 201);
    }
}
