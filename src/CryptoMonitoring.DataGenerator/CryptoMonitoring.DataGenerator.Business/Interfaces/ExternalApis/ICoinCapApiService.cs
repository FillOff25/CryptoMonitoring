namespace CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;

public interface ICoinCapApiService
{
    Task GetCryptoCurrenciesAsync();
}