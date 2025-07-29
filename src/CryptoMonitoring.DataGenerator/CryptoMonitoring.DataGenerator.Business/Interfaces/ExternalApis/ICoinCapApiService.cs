using CryptoMonitoring.Common.DTOs.CoinCapApi;

namespace CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;

public interface ICoinCapApiService
{
    Task<GetCoinCapDataResponseDto> GetCryptoCurrencyAsync();
    Task<GetCoinCapDataResponseDto> GetMarketDataAsync();
    Task<GetCoinCapDataResponseDto> GetPriceHistoryByIdAsync(CoinCapHistoryDataRequestDto dto);
}