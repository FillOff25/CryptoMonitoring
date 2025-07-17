using CryptoMonitoring.Models.Entities;

namespace CryptoMonitoring.DataGenerator.Persistence.Interfaces;

public interface ICryptoCurrenciesRepository : IGenericRepository<CryptoCurrency, Guid>
{
    Task<bool> IsNameExistAsync(string name);
}