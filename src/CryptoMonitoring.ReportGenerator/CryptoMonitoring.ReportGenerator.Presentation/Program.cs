using CryptoMonitoring.Common.Extensions;
using CryptoMonitoring.Common.Middlewares;
using CryptoMonitoring.Common.Persistence;
using CryptoMonitoring.ReportGenerator.Business;
using Serilog;

DotNetEnv.Env.Load();

Log.Logger = new LoggerConfiguration()
    .ConfigureLogger(Environment.GetEnvironmentVariable("CRYPTO_REPORTGENERATOR_MONITORING_LOGS_DB_CONNECTION_STRING")!);

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration.AddEnvironmentVariables();

    builder.Services.AddSerilog();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddPersistence(builder.Configuration);
    builder.Services.AddServices();
    builder.Services.AddCommands();
    builder.Services.AddRedis(builder.Configuration);

    var app = builder.Build();

    app.UseMiddleware<ExceptionMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    await app.ApplyMigrationsAsync();

    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

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