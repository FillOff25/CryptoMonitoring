using Newtonsoft.Json;

namespace CryptoMonitoring.Common.DTOs.CoinCapApi;

public class CoinCapAssetResponseDto
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("symbol")]
    public string Symbol { get; set; } = string.Empty;

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("supply")]
    public string? Supply { get; set; }

    [JsonProperty("marketCapUsd")]
    public string? MarketCapUsd { get; set; }

    [JsonProperty("volumeUsd24Hr")]
    public string? VolumeUsd24Hr { get; set; }

    [JsonProperty("changePercent24Hr")]
    public string? ChangePercent24Hr { get; set; }

    [JsonProperty("vwap24Hr")]
    public string? Vwap24Hr { get; set; }
}