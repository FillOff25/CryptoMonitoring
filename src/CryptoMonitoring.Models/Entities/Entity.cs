namespace CryptoMonitoring.Models.Entities;

public class Entity<TKey>
{
    public required TKey Id { get; set; }
}