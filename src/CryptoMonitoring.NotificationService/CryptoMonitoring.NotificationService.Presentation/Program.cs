using CryptoMonitoring.Common.Extensions;
using CryptoMonitoring.Common.Middlewares;
using CryptoMonitoring.Common.Persistence;
using CryptoMonitoring.NotificationService.Business;
using Microsoft.OpenApi.Models;
using Serilog;

DotNetEnv.Env.Load();

Log.Logger = new LoggerConfiguration()
    .ConfigureLogger(Environment.GetEnvironmentVariable("CRYPTO_NOTIFICATIONSERVICE_MONITORING_LOGS_DB_CONNECTION_STRING")!);

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddSerilog();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddAuth(builder.Configuration);
    builder.Services.AddPersistence(builder.Configuration);
    builder.Services.AddRedis(builder.Configuration);
    builder.Services.AddAutoMapper();
    builder.Services.AddCommonServices();
    builder.Services.AddServices();
    builder.Services.AddCommands();

    var app = builder.Build();

    app.UseMiddleware<ExceptionMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    await app.ApplyMigrationsAsync();
    await app.SeedDataAsync(builder.Configuration);

    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.MapControllers();

    app.UseAuthentication();
    app.UseAuthorization();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}