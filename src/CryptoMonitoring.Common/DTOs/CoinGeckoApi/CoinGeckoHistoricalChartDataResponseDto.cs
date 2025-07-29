using Newtonsoft.Json;

namespace CryptoMonitoring.Common.DTOs.CoinGeckoApi;

public class CoinGeckoHistoricalChartDataResponseDto
{
    [JsonProperty("prices")]
    public List<List<decimal>> Prices { get; set; } = new List<List<decimal>>();
}
