namespace CryptoMonitoring.Models.Entities;

public class MarketData : Entity<Guid>
{
    public DateTime Timestamp { get; set; }
    public decimal? Volume24hUsd { get; set; }
    public decimal? MarketCapUsd { get; set; }
    public decimal? Vwap24hUsd { get; set; }
    public decimal? Change24hPercent { get; set; }

    public Guid CryptoCurrencyId { get; set; }
    public CryptoCurrency CryptoCurrency { get; set; } = null!;
}