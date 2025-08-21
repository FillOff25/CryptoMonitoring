namespace CryptoMonitoring.Models.Entities;

public class User : Entity<Guid>
{
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public string Role { get; set; } = "User";
    public long TelegramId { get; set; }
    public bool IsEmailNotify { get; set; }
    public bool IsTelegramNotify { get; set; }
    public bool IsActive { get; set; } = true;
}