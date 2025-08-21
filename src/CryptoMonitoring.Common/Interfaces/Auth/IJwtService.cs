using CryptoMonitoring.Models.Entities;

namespace CryptoMonitoring.Common.Interfaces.Auth;

public interface IJwtService
{
    string GenerateToken(User user);
    Guid GetUserIdFromToken(string authHeader);
}