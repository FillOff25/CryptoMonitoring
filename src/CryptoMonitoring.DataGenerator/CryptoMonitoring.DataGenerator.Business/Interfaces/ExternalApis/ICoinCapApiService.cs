namespace CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;

public interface ICoinCapApiService
{
    Task GetCryptoCurrencyAsync();
    Task GetMarketDataAsync();
}