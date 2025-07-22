using CryptoMonitoring.DataGenerator.Business.DTOs.CoinCapApi;

namespace CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;

public interface ICoinCapApiService
{
    Task GetCryptoCurrencyAsync();
    Task GetMarketDataAsync();
    Task GetPriceHistoryAsync(CoinCapHistoryDataRequestDto dto);
}