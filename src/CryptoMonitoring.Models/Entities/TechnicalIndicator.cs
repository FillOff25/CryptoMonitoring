using CryptoMonitoring.Models.Enums;

namespace CryptoMonitoring.Models.Entities;

public class TechnicalIndicator
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public IndicatorTypeEnum IndicatorType { get; set; }
    public decimal Value { get; set; }

    public Guid CryptoCurrencyId { get; set; }
    public CryptoCurrency CryptoCurrency { get; set; } = null!;
}
