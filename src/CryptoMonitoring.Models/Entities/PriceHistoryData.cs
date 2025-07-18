namespace CryptoMonitoring.Models.Entities;

public class PriceHistoryData : Entity<Guid>
{
    public DateTime Timestamp { get; set; }
    public decimal PriceUsd { get; set; }
    
    public Guid CryptoCurrencyId { get; set; }
    public CryptoCurrency CryptoCurrency { get; set; } = null!;
}
