using CryptoMonitoring.Models.Entities;

namespace CryptoMonitoring.DataGenerator.DataAccess.Interfaces;

public interface IGenericRepository<TEntity, TKey> 
    where TEntity : Entity<TKey>
{
    Task Add(TEntity entity);
    Task Delete(TKey id);
    IQueryable<TEntity> GetAll();
    Task<TEntity?> GetById(TKey id);
    Task Update(TEntity entity);
}