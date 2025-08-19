using CryptoMonitoring.Common.Persistence.Databases;
using CryptoMonitoring.Common.Persistence.Interfaces;
using CryptoMonitoring.Common.Persistence.Repositories;

namespace CryptoMonitoring.Common.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly CryptoMonitoringDataDbContext _dbContext;

    private ICryptoCurrenciesRepository? _cryptoCurrenciesRepository;
    private IMarketDataRepository? _marketDataRepository;
    private IPriceHistoryDataRepository? _priceHistoryDataRepository;
    private ITechnicalIndicatorsRepository? _technicalIndicatorsRepository;
    private IExcelReportsRepository? _excelReportsRepository;
    private IUsersRepository? _usersRepository;

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

    public IPriceHistoryDataRepository PriceHistoryData
    {
        get
        {
            _priceHistoryDataRepository ??= new PriceHistoryDataRepository(_dbContext);
            return _priceHistoryDataRepository;
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

    public IUsersRepository Users
    {
        get
        {
            _usersRepository ??= new UsersRepository(_dbContext);
            return _usersRepository;
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