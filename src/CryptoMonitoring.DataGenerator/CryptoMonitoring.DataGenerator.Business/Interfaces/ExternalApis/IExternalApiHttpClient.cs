namespace CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis
{
    public interface IExternalApiHttpClient
    {
        void SetBaseAddress(string baseAddress);
        void SetBearerApiKey(string apiKey);
        Task<T?> GetDataAsync<T>(string endpoint);
    }
}