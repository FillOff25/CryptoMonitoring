using CryptoMonitoring.Common.DTOs.CoinCapApi;

namespace CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;

public interface ICoinCapApiService
{
    Task<CoinCapResponseDto<List<CoinCapAssetResponseDto>>> GetCryptoCurrencyAsync();
    Task<CoinCapResponseDto<List<CoinCapAssetResponseDto>>> GetMarketDataAsync();
    Task<CoinCapPriceHistoryDataRabbitMQResponseDto> GetPriceHistoryByIdAsync(CoinCapHistoryDataRequestDto dto);
}