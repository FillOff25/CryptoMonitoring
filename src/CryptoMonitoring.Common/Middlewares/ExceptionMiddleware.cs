using CryptoMonitoring.Common.DTOs;
using CryptoMonitoring.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;

namespace CryptoMonitoring.Common.Middlewares;

public class ExceptionMiddleware
{
    public readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var message = string.Empty;

        try
        {
            await _next(context);
        }
        catch (HttpException ex)
        {
            context.Response.StatusCode = ex.ErrorCode;
            message = ex.Message;
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            message = ex.Message;
        }
        finally
        {
            switch (context.Response.StatusCode)
            {
                case 401:
                    message = "Unauthorized";
                    break;
                case 403:
                    message = "Forbidden";
                    break;
            }

            switch (context.Response.StatusCode)
            {
                case 200:
                case 201:
                    break;
                default:
                    var response = new ResponseDto<object>()
                    {
                        Result = null,
                        IsFailure = true,
                        Message = message,
                        StatusCode = context.Response.StatusCode
                    };

                    Log.Error(response.Message);

                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                        
                    break;
            }
        }
    }
}
