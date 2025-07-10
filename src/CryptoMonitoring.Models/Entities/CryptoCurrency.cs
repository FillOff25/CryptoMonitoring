namespace CryptoMonitoring.Models.Entities;

public class CryptoCurrency
{
    public required string Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public int Rank { get; set; }
    public decimal Supply { get; set; }
    public decimal? MaxSupply { get; set; }
}