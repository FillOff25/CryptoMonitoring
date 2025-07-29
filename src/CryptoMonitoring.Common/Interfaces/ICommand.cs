namespace CryptoMonitoring.Common.Interfaces;

public interface ICommand<TRequest, TResponse>
{
    Task<TResponse> ExecuteAsync(TRequest request);
}
