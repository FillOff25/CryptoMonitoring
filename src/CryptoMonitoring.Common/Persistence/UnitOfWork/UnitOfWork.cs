using CryptoMonitoring.Common.Persistence.Databases;
using CryptoMonitoring.Common.Persistence.Interfaces;
using CryptoMonitoring.Common.Persistence.Repositories;
using CryptoMonitoring.Models.Entities;

namespace CryptoMonitoring.Common.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly CryptoMonitoringDataDbContext _dbContext;

    private ICryptoCurrenciesRepository? _cryptoCurrenciesRepository;
    private IMarketDataRepository? _marketDataRepository;
    private IGenericRepository<TechnicalIndicator, Guid>? _technicalIndicatorsRepository;
    private IPriceHistoryDataRepository? _priceHistoryDataRepository;

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

    public IGenericRepository<TechnicalIndicator, Guid> TechnicalIndicators
    {
        get
        {
            _technicalIndicatorsRepository ??= new GenericRepository<TechnicalIndicator, Guid>(_dbContext);
            return _technicalIndicatorsRepository;
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