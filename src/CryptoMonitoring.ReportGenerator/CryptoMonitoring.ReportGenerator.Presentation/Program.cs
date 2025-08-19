using CryptoMonitoring.Common.Extensions;
using CryptoMonitoring.Common.Middlewares;
using CryptoMonitoring.Common.Persistence;
using CryptoMonitoring.ReportGenerator.Business;
using Serilog;
using System.Text.Json.Serialization;

DotNetEnv.Env.Load();

Log.Logger = new LoggerConfiguration()
    .ConfigureLogger(Environment.GetEnvironmentVariable("CRYPTO_REPORTGENERATOR_MONITORING_LOGS_DB_CONNECTION_STRING")!);

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration.AddEnvironmentVariables();

    builder.Services.AddSerilog();
    builder.Services.AddControllers()
        .AddJsonOptions(opt =>
        {
            opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddPersistence(builder.Configuration);
    builder.Services.AddServices();
    builder.Services.AddCommands();
    builder.Services.AddRedis(builder.Configuration);
    builder.Services.AddCommonServices();

    var app = builder.Build();

    app.UseMiddleware<ExceptionMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    await app.ApplyMigrationsAsync();

    app.UseStaticFiles();
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