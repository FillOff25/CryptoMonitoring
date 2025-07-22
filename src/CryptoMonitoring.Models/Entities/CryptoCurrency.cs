namespace CryptoMonitoring.Models.Entities;

public class CryptoCurrency : Entity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;

    public string? CoinCapId { get; set; }
}