using CryptoMonitoring.DataGenerator.Persistence.Databases;
using CryptoMonitoring.DataGenerator.Persistence.Interfaces;
using CryptoMonitoring.DataGenerator.Persistence.Repositories;
using CryptoMonitoring.Models.Entities;

namespace CryptoMonitoring.DataGenerator.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly CryptoMonitoringDataDbContext _dbContext;

    private ICryptoCurrenciesRepository? _cryptoCurrenciesRepository;
    private IGenericRepository<MarketData, Guid>? _marketDatasRepository;
    private IGenericRepository<TechnicalIndicator, Guid>? _technicalIndicatorsRepository;

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

    public IGenericRepository<MarketData, Guid> MarketDatas
    {
        get
        {
            _marketDatasRepository ??= new GenericRepository<MarketData, Guid>(_dbContext);
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