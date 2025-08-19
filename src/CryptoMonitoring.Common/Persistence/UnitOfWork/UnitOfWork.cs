using CryptoMonitoring.Common.Persistence.Databases;
using CryptoMonitoring.Common.Persistence.Interfaces;
using CryptoMonitoring.Common.Persistence.Repositories;

namespace CryptoMonitoring.Common.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly CryptoMonitoringDataDbContext _dbContext;

    private ICryptoCurrenciesRepository? _cryptoCurrenciesRepository;
    private IMarketDataRepository? _marketDataRepository;
    private ITechnicalIndicatorsRepository? _technicalIndicatorsRepository;
    private IPriceHistoryDataRepository? _priceHistoryDataRepository;
    private IExcelReportsRepository? _excelReportsRepository;

    public UnitOfWork(CryptoMonitoringDataDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ICryptoCurrenciesRepository CryptoCurrencies
    {
        get
        {
            _cryptoCurrenciesRepository ??= new CryptoCurrenciesRepository(_dbContext);
            return _cryptoCurrenciesRepository;
        }
    }

    public IMarketDataRepository MarketData
    {
        get
        {
            _marketDataRepository ??= new MarketDataRepository(_dbContext);
            return _marketDataRepository;
        }
    }

    public ITechnicalIndicatorsRepository TechnicalIndicators
    {
        get
        {
            _technicalIndicatorsRepository ??= new TechnicalIndicatorsRepository(_dbContext);
            return _technicalIndicatorsRepository;
        }
    }

    public IExcelReportsRepository ExcelReports
    {
        get
        {
            _excelReportsRepository ??= new ExcelReportsRepository(_dbContext);
            return _excelReportsRepository;
        }
    }

    public IPriceHistoryDataRepository PriceHistoryData
    {
        get
        {
            _priceHistoryDataRepository ??= new PriceHistoryDataRepository(_dbContext);
            return _priceHistoryDataRepository;
        }
    }

    public async Task SaveAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}