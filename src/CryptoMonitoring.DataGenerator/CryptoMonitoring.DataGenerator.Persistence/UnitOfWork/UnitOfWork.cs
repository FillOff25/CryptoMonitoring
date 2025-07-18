using CryptoMonitoring.DataGenerator.Persistence.Databases;
using CryptoMonitoring.DataGenerator.Persistence.Interfaces;
using CryptoMonitoring.DataGenerator.Persistence.Repositories;
using CryptoMonitoring.Models.Entities;

namespace CryptoMonitoring.DataGenerator.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly CryptoMonitoringDataDbContext _dbContext;

    private ICryptoCurrenciesRepository? _cryptoCurrenciesRepository;
    private IMarketDatasRepository? _marketDatasRepository;
    private IGenericRepository<TechnicalIndicator, Guid>? _technicalIndicatorsRepository;
    private IGenericRepository<PriceHistoryData, Guid>? _priceHistoryDatasRepository;

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

    public IMarketDatasRepository MarketDatas
    {
        get
        {
            _marketDatasRepository ??= new MarketDatasRepository(_dbContext);
            return _marketDatasRepository;
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

    public IGenericRepository<PriceHistoryData, Guid> PriceHistoryDatas
    {
        get
        {
            _priceHistoryDatasRepository ??= new GenericRepository<PriceHistoryData, Guid>(_dbContext);
            return _priceHistoryDatasRepository;
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