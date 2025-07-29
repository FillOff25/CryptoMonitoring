using CryptoMonitoring.Common.DTOs.CoinGeckoApi;
using CryptoMonitoring.Common.Extensions;
using CryptoMonitoring.Common.Interfaces;
using CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitoring.DataGenerator.Business.Commands.ExternalApis.CoinGeckoApi;

public class GetPriceHistoryDataCoinGeckoCommand : ICommand<CoinGeckoHistoricalChartDataRequestDto, IActionResult>
{
    private readonly ICoinGeckoApiService _coinGeckoApiService;

    public GetPriceHistoryDataCoinGeckoCommand(ICoinGeckoApiService coinGeckoApiService)
    {
        _coinGeckoApiService = coinGeckoApiService;
    }

    public async Task<IActionResult> ExecuteAsync(CoinGeckoHistoricalChartDataRequestDto request)
    {
        return (await _coinGeckoApiService.GetPriceHistoryDataByIdAsync(request))
            .ToHttpResponse("Crypto currency data from CoinGecko Api saved successfully", 201);
    }
}
