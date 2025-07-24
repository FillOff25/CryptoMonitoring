using Newtonsoft.Json;

namespace CryptoMonitoring.DataGenerator.Business.DTOs.CoinGeckoApi;

public class CoinGeckoCoinWithMarketDataResponseDto
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("market_cap")]
    public decimal? MarketCap { get; set; }

    [JsonProperty("total_volume")]
    public decimal? TotalVolume { get; set; }

    [JsonProperty("price_change_percentage_24h")]
    public decimal? PriceChangePercentage24h { get; set; }

    [JsonProperty("circulating_supply")]
    public decimal? CirculatingSupply { get; set; }

    [JsonProperty("last_updated")]
    public DateTime LastUpdated { get; set; }
}
