namespace CryptoMonitoring.Models.Entities;

public class Market
{
    public required string ExchangeId { get; set; }
    public required string BaseSymbol { get; set; }
    public required string QuoteSymbol { get; set; }
    public decimal PriceUsd { get; set; }
    public decimal VolumeUsd24Hr { get; set; }
    public decimal VolumePercent { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string CryptoCurrencyId { get; set; } = string.Empty;
    public CryptoCurrency CryptoCurrency { get; set; } = null!;
}