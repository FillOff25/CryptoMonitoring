using CryptoMonitoring.Models.Entities;

namespace CryptoMonitoring.Common.Persistence.Interfaces;

public interface IGenericRepository<TEntity, TKey> 
    where TEntity : Entity<TKey>
{
    Task AddAsync(TEntity entity);
    Task DeleteAsync(TKey id);
    IQueryable<TEntity> GetAll();
    Task<TEntity?> GetByIdAsync(TKey id);
    void Update(TEntity entity);
}