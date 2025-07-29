namespace CryptoMonitoring.Common.DTOs;

public class ResponseDto<T>
{
    public T? Result { get; set; }
    public bool IsFailure { get; set; }
    public string Message { get; set; } = string.Empty;
    public int StatusCode { get; set; }
}
