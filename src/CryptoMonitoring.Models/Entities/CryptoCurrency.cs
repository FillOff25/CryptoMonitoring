namespace CryptoMonitoring.Models.Entities;

public class CryptoCurrency
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
}