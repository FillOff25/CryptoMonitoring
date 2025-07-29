using CryptoMonitoring.Common.DTOs.CoinGeckoApi;
using CryptoMonitoring.Common.Extensions;
using CryptoMonitoring.Common.Interfaces;
using CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitoring.DataGenerator.Business.Commands.ExternalApis.CoinGeckoApi;

public class GetMarketDataCoinGeckoCommand : ICommand<CoinGeckoCoinWithMarketDataRequestDto, IActionResult>
{
    private readonly ICoinGeckoApiService _coinGeckoApiService;

    public GetMarketDataCoinGeckoCommand(ICoinGeckoApiService coinGeckoApiService)
    {
        _coinGeckoApiService = coinGeckoApiService;
    }

    public async Task<IActionResult> ExecuteAsync(CoinGeckoCoinWithMarketDataRequestDto request)
    {
        return (await _coinGeckoApiService.GetMarketDataByIdAsync(request))
            .ToHttpResponse("Crypto currency data from CoinGecko Api saved successfully", 201);
    }
}
