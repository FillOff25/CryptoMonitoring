using CryptoMonitoring.ReportGenerator.Business.Commands;
using CryptoMonitoring.ReportGenerator.Business.Interfaces;
using CryptoMonitoring.ReportGenerator.Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoMonitoring.ReportGenerator.Business;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IExcelReportService, ExcelReportService>();

        return services;
    }

    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddScoped<GenerateDailyExcelReportCommand>();
        services.AddScoped<GenerateTechnicalAnalysisReportCommand>();

        return services;
    }
}
