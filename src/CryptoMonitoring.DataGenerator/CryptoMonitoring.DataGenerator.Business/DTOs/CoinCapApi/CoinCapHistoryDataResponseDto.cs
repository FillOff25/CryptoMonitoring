using Newtonsoft.Json;

namespace CryptoMonitoring.DataGenerator.Business.DTOs.CoinCapApi;

public class CoinCapHistoryDataResponseDto
{
    [JsonProperty("priceUsd")]
    public string PriceUsd { get; set; } = string.Empty;

    [JsonProperty("time")]
    public long Time { get; set; }

    [JsonProperty("date")]
    public DateTime Date { get; set; }

    [JsonProperty("circulatingSupply")]
    public string? CirculatingSupply { get; set; }
}
