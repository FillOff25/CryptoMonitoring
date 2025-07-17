using CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;
using Newtonsoft.Json;
using Serilog;
using System.Net.Http.Headers;

namespace CryptoMonitoring.DataGenerator.Business.Services.ExternalApis;

public class ExternalApiHttpClient : IExternalApiHttpClient
{
    private readonly HttpClient _httpClient;

    public ExternalApiHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public void SetBaseAddress(string baseAddress)
    {
        _httpClient.BaseAddress = new Uri(baseAddress);
    }

    public void SetBearerApiKey(string apiKey)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", apiKey);
    }

    public async Task<T?> GetDataAsync<T>(string endpoint)
    {
        try
        {
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var entities = JsonConvert.DeserializeObject<T>(content);

            Log.Information("Data fetched successfully from endpoint: {Endpoint}", endpoint);

            return entities;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while fetching data from endpoint: {Endpoint}", endpoint);
            throw;
        }
    }
}
