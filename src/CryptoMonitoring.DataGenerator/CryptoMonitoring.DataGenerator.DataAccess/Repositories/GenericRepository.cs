using CryptoMonitoring.DataGenerator.DataAccess.Databases;
using CryptoMonitoring.DataGenerator.DataAccess.Interfaces;
using CryptoMonitoring.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoMonitoring.DataGenerator.DataAccess.Repositories;

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
        return await DbSet.AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id!.Equals(id));
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
        await DbContext.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        DbSet.Update(entity);
        await DbContext.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(TKey id)
    {
        await DbSet.Where(e => e.Id!.Equals(id))
            .ExecuteDeleteAsync();
    }
}