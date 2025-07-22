namespace CryptoMonitoring.DataGenerator.Business.DTOs.CoinCapApi;

public class CoinCapHistoryDataRequestDto
{
    public required string Slug { get; set; }
    public required string Interval { get; set; } // m1 m5 m30 h1 h2 h6 h12 d1
}
