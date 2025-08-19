using CryptoMonitoring.Common.Persistence.Databases;
using CryptoMonitoring.Common.Persistence.Interfaces;
using CryptoMonitoring.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoMonitoring.Common.Persistence.Repositories;

public class UsersRepository : GenericRepository<User, Guid>, IUsersRepository
{
    public UsersRepository(CryptoMonitoringDataDbContext dbContext)
        : base(dbContext)
    { }

    public async Task<User?> GetByEmail(string email)
    {
        return await DbSet.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }
}
