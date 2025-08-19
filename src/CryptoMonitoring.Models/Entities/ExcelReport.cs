using CryptoMonitoring.Models.Enums;

namespace CryptoMonitoring.Models.Entities;

public class ExcelReport : Entity<Guid>
{
    public string FilePath { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public ExcelReportTypeEnum ReportType { get; set; }
    public DateTime CreatedAt { get; set; }

    public Guid CryptoCurrencyId { get; set; }
    public CryptoCurrency CryptoCurrency { get; set; } = null!;
}