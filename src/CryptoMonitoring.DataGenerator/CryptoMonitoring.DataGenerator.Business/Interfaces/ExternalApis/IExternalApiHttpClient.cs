namespace CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;

public interface IExternalApiHttpClient
{
    void SetBaseAddress(string baseAddress);
    void SetHeaderApiKey(string header, string value);
    Task<T?> GetDataAsync<T>(string endpoint);
}