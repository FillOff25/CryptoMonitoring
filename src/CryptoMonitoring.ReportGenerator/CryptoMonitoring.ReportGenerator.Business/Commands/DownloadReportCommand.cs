using CryptoMonitoring.Common.DTOs.ReportGenerator;
using CryptoMonitoring.Common.Interfaces;
using CryptoMonitoring.ReportGenerator.Business.Interfaces;

namespace CryptoMonitoring.ReportGenerator.Business.Commands;

public class DownloadReportCommand : ICommand<DownloadReportRequestDto, DownloadReportResponseDto>
{
    private readonly IExcelReportService _excelReportService;

    public DownloadReportCommand(IExcelReportService excelReportService)
    {
        _excelReportService = excelReportService;
    }

    public async Task<DownloadReportResponseDto> ExecuteAsync(DownloadReportRequestDto request)
    {
        return await _excelReportService.DownloadReportAsync(request);
    }
}