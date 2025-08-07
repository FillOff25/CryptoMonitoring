using CryptoMonitoring.Common.Persistence.Databases;
using CryptoMonitoring.Common.Persistence.Interfaces;
using CryptoMonitoring.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoMonitoring.Common.Persistence.Repositories;

public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> 
    where TEntity : Entity<TKey>
{
    protected readonly CryptoMonitoringDataDbContext DbContext;
    protected readonly DbSet<TEntity> DbSet;

    public GenericRepository(CryptoMonitoringDataDbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = dbContext.Set<TEntity>();
    }

    public virtual IQueryable<TEntity> GetAll()
    {
        return DbSet.AsNoTracking();
    }

    public virtual async Task<TEntity?> GetByIdAsync(TKey id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
    }

    public virtual void Update(TEntity entity)
    {
        DbSet.Update(entity);
    }

    public virtual async Task DeleteAsync(TKey id)
    {
        var entity = await DbSet.FindAsync(id);

        if (entity != null)
        {
            DbSet.Remove(entity);
        }
    }
}