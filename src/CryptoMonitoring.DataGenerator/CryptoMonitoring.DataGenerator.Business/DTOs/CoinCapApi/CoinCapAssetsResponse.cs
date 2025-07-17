using Newtonsoft.Json;

namespace CryptoMonitoring.DataGenerator.Business.DTOs.CoinCapApi;

public class CoinCapAssetsResponse
{
    [JsonProperty("data")]
    public List<CoinCapAssetDto> Data { get; set; } = new List<CoinCapAssetDto>();
}
