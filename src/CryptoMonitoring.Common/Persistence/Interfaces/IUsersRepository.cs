using CryptoMonitoring.Models.Entities;

namespace CryptoMonitoring.Common.Persistence.Interfaces;

public interface IUsersRepository : IGenericRepository<User, Guid>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByTelegramIdAsync(long telegramId);
}