using CryptoMonitoring.Common.DTOs.CoinGeckoApi;

namespace CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;

public interface ICoinGeckoApiService
{
    Task<GetCoinGeckoDataResponseDto> GetCryptoCurrencyAsync();
    Task<GetCoinGeckoMarketDataByIdResponseDto> GetMarketDataByIdAsync(CoinGeckoCoinWithMarketDataRequestDto dto);
    Task<GetCoinGeckoPriceHistoryDataByIdResponseDto> GetPriceHistoryDataByIdAsync(CoinGeckoHistoricalChartDataRequestDto dto);
}