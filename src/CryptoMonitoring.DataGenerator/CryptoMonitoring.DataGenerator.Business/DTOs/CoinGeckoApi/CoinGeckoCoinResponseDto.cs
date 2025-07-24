using Newtonsoft.Json;

namespace CryptoMonitoring.DataGenerator.Business.DTOs.CoinGeckoApi;

public class CoinGeckoCoinResponseDto
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("symbol")]
    public string Symbol { get; set; } = string.Empty;

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
}
