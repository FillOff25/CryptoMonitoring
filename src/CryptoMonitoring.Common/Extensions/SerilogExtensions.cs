using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace CryptoMonitoring.Common.Extensions;

public static class SerilogExtensions
{
    public static Logger ConfigureLogger(this LoggerConfiguration loggerConfiguration, string mongoDatabaseUrl)
    {
        return loggerConfiguration
            .MinimumLevel.Override("Default", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .WriteTo.Console()
            .WriteTo.MongoDB(mongoDatabaseUrl)
            .CreateLogger();
    }
}
