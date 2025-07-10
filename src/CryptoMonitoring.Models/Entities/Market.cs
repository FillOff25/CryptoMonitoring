namespace CryptoMonitoring.Models.Entities;

public class Market
{
    public required string MarketId { get; set; }
    public required string BaseSymbol { get; set; }
    public required string QuoteSymbol { get; set; }
    public decimal PriceUsd { get; set; }
    public decimal VolumeUsd24Hr { get; set; }
    public DateTime UpdatedAt { get; set; }
}