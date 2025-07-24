using CryptoMonitoring.DataGenerator.Business.DTOs.CoinGeckoApi;

namespace CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis
{
    public interface ICoinGeckoApiService
    {
        Task GetCryptoCurrencyAsync();
        Task GetMarketDataByIdAsync(CoinGeckoCoinWithMarketDataRequestDto dto);
        Task GetPriceHistoryDataByIdAsync(CoinGeckoHistoricalChartDataRequestDto dto);
    }
}