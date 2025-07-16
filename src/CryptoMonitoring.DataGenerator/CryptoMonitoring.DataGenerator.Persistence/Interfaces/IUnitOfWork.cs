namespace CryptoMonitoring.DataGenerator.Persistence.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        Task SaveAsync();
    }
}