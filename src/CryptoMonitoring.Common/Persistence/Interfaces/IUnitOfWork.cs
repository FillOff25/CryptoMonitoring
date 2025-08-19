using CryptoMonitoring.Models.Entities;

namespace CryptoMonitoring.Common.Persistence.Interfaces;

public interface IUnitOfWork : IAsyncDisposable
{
    ICryptoCurrenciesRepository CryptoCurrencies { get; }
    IMarketDataRepository MarketData { get; }
    IPriceHistoryDataRepository PriceHistoryData { get; }
    ITechnicalIndicatorsRepository TechnicalIndicators { get; }
    IExcelReportsRepository ExcelReports { get; }
    IUsersRepository Users { get; }

    Task SaveAsync();
}