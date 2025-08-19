namespace CryptoMonitoring.Common.DTOs.ReportGenerator;

public record GenerateComparativeAnalysisReportRequestDto(
    List<GetCryptoCurrencyRequestDto> CryptoCurrencies,
    int Period = 30);