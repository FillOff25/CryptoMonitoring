using CryptoMonitoring.Common.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitoring.Common.Extensions;

public static class ResponseExtensions
{
    public static IActionResult ToHttpResponse<T>(this T obj, string message, int statusCode)
    {
        var responseDto = new ResponseDto<T>
        {
            Result = obj,
            IsFailure = false,
            Message = message,
            StatusCode = statusCode,
        };

        return new ObjectResult(responseDto)
        {
            StatusCode = statusCode,
        };
    }
}
