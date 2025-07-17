using Newtonsoft.Json;

namespace CryptoMonitoring.DataGenerator.Business.DTOs.CoinCapApi;

public class CoinCapAssetDto
{
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("symbol")]
    public string Symbol { get; set; } = string.Empty;
}