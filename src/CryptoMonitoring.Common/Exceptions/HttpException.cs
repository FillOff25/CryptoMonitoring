namespace CryptoMonitoring.Common.Exceptions;

public class HttpException : Exception
{
    public int ErrorCode { get; set; } = 500;

    public HttpException(string message, int errorCode)
        : base(message)
    {
        ErrorCode = errorCode;
    }
}
