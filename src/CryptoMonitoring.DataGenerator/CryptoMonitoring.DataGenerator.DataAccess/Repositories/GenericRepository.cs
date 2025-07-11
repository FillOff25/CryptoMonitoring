using Microsoft.EntityFrameworkCore;

namespace CryptoMonitoring.DataGenerator.DataAccess.Repositories;

public class GenericRepository<TEntity, TKey> 
    where TEntity : class
{
    protected readonly CryptoMonitoringDataDbContext DbContext;
    protected readonly DbSet<TEntity> DbSet;

    public GenericRepository(CryptoMonitoringDataDbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = dbContext.Set<TEntity>();
    }
}
