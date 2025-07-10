namespace CryptoMonitoring.Models.Entities;

public class PriceHistory
{
    public required string CryptoCurrencyId { get; set; }
    public required DateTime Timestamp { get; set; }
    public decimal PriceUsd { get; set; }

    public CryptoCurrency CryptoCurrency { get; set; } = null!;
}
