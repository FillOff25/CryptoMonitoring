using CryptoMonitoring.Common.DTOs.CoinGeckoApi;

namespace CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;

public interface ICoinGeckoApiService
{
    Task<List<CoinGeckoCoinResponseDto>> GetCryptoCurrencyAsync();
    Task<CoinGeckoMarketDataByIdRabbitMQResponseDto> GetMarketDataByIdAsync(CoinGeckoCoinWithMarketDataRequestDto dto);
    Task<CoinGeckoPriceHistoryDataByIdRabbitMQResponseDto> GetPriceHistoryDataByIdAsync(CoinGeckoHistoricalChartDataRequestDto dto);
}