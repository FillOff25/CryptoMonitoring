namespace CryptoMonitoring.Models.Entities;

public class User : Entity<Guid>
{
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
}