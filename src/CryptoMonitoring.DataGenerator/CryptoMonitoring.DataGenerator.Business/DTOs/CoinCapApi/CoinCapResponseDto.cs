using Newtonsoft.Json;

namespace CryptoMonitoring.DataGenerator.Business.DTOs.CoinCapApi;

public class CoinCapResponseDto<T>
{
    [JsonProperty("data")]
    public T? Data { get; set; }

    [JsonProperty("error")]
    public string? Error { get; set; }

    [JsonProperty("timestamp")]
    public long Timestamp { get; set; }
}