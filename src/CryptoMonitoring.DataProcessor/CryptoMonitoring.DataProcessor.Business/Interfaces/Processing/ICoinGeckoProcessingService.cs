using CryptoMonitoring.Common.DTOs.CoinGeckoApi;

namespace CryptoMonitoring.DataProcessor.Business.Interfaces.Processing
{
    public interface ICoinGeckoProcessingService
    {
        Task<GetCoinGeckoDataResponseDto> ProcessCryptoCurrencyAsync(List<CoinGeckoCoinResponseDto> dto);
        Task<GetCoinGeckoMarketDataByIdResponseDto> ProcessMarketDataByIdAsync(CoinGeckoMarketDataByIdRabbitMQResponseDto dto);
        Task<GetCoinGeckoPriceHistoryDataByIdResponseDto> ProcessPriceHistoryDataByIdAsync(CoinGeckoPriceHistoryDataByIdRabbitMQResponseDto dto);
    }
}