using ClosedXML.Excel;
using CryptoMonitoring.Common.DTOs.ReportGenerator;
using CryptoMonitoring.Common.Exceptions;
using CryptoMonitoring.Common.Interfaces.Entities;
using CryptoMonitoring.Common.Persistence.Interfaces;
using CryptoMonitoring.Models.Entities;
using CryptoMonitoring.Models.Enums;
using CryptoMonitoring.ReportGenerator.Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CryptoMonitoring.ReportGenerator.Business.Services;

public class ExcelReportService : IExcelReportService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICryptoCurrencyService _cryptoCurrencyService;
    private readonly string _reportsStoragePath;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ExcelReportService(
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor,
        ICryptoCurrencyService cryptoCurrencyService)
    {
        _unitOfWork = unitOfWork;
        _reportsStoragePath = Path.Combine(Directory.GetCurrentDirectory(), configuration["REPORT_PATH"]!);
        _httpContextAccessor = httpContextAccessor;
        _cryptoCurrencyService = cryptoCurrencyService;
    }

    public async Task<GenerateReportResponseDto> GenerateDailyReportAsync(GenerateDailyReportRequestDto dto)
    {
        var cryptoCurrency = await _cryptoCurrencyService.GetCryptoCurrencyWithCache(dto.Name, dto.Symbol);

        var createdAt = DateTime.UtcNow;
        var existExcelReport = await _unitOfWork.ExcelReports.GetReportAsync(cryptoCurrency.Id, ExcelReportTypeEnum.DailyReport, createdAt);
        if (existExcelReport != null)
        {
            return new GenerateReportResponseDto(existExcelReport.Id, existExcelReport.OriginalFileName, existExcelReport.ReportType, existExcelReport.CreatedAt);
        }

        var priceHistoryData = await _unitOfWork.PriceHistoryData.GetByCryptoCurrencyId(cryptoCurrency.Id)
            .Where(phd => phd.Timestamp.Date == DateTime.UtcNow.Date)
            .OrderBy(phd => phd.Timestamp)
            .ToListAsync();

        var marketData = await _unitOfWork.MarketData.GetByCryptoCurrencyId(cryptoCurrency.Id)
            .Where(md => md.Timestamp.Date == DateTime.UtcNow.Date)
            .OrderBy(md => md.Timestamp)
            .ToListAsync();

        if (priceHistoryData.Count == 0 || marketData.Count == 0)
        {
            throw new HttpException("Data hasn't been uploaded", 400);
        }

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add($"Daily Report - {cryptoCurrency.Name} ({cryptoCurrency.Symbol})");

        worksheet.Cell(1, 1).Value = "Date";
        worksheet.Cell(1, 2).Value = "Cryptocurrency (symbol)";
        worksheet.Cell(1, 3).Value = "Closing price (USD)";
        worksheet.Cell(1, 4).Value = "Volume 24ч (USD)";
        worksheet.Cell(1, 5).Value = "Change 24ч (%)";
        worksheet.Cell(1, 6).Value = "Market Cap (USD)";

        var headerRange = worksheet.Range("A1:F1");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        int row = 2;
        foreach (var ph in priceHistoryData)
        {
            var md = marketData.FirstOrDefault(m => m.Timestamp.Date == ph.Timestamp.Date);

            worksheet.Cell(row, 1).Value = ph.Timestamp.ToString("yyyy-MM-dd HH:mm:ss");
            worksheet.Cell(row, 2).Value = $"{cryptoCurrency.Name} ({cryptoCurrency.Symbol})";
            worksheet.Cell(row, 3).Value = ph.PriceUsd;
            worksheet.Cell(row, 4).Value = md?.Volume24hUsd;
            worksheet.Cell(row, 5).Value = md?.Change24hPercent;
            worksheet.Cell(row, 6).Value = md?.MarketCapUsd;
            row++;
        }

        worksheet.Column(1).Style.DateFormat.SetFormat("yyyy-MM-dd HH:mm:ss");
        worksheet.Column(3).Style.NumberFormat.Format = "$#,##0.00";
        worksheet.Column(4).Style.NumberFormat.Format = "#,##0";
        worksheet.Column(5).Style.NumberFormat.Format = "0.000\\%";
        worksheet.Column(6).Style.NumberFormat.Format = "$#,##0";

        worksheet.Columns().AdjustToContents();

        var excelReport = await SaveFileAsync(
            workbook, 
            $"Daily_Report_{dto.Name}_{dto.Symbol}_{createdAt:yyyy-MM-dd}", 
            ExcelReportTypeEnum.DailyReport,
            createdAt, 
            cryptoCurrency.Id);

        return new GenerateReportResponseDto(excelReport.Id, excelReport.OriginalFileName, excelReport.ReportType, excelReport.CreatedAt);
    }

    public async Task<GenerateReportResponseDto> GenerateTechnicalAnalysisReportAsync(GenerateTechnicalAnalysisReportRequestDto dto)
    {
        var cryptoCurrency = await _cryptoCurrencyService.GetCryptoCurrencyWithCache(dto.Name, dto.Symbol);

        var createdAt = DateTime.UtcNow;
        var existExcelReport = await _unitOfWork.ExcelReports.GetReportAsync(cryptoCurrency.Id, ExcelReportTypeEnum.TechnicalAnalisis, createdAt);
        if (existExcelReport != null)
        {
            return new GenerateReportResponseDto(existExcelReport.Id, existExcelReport.OriginalFileName, existExcelReport.ReportType, existExcelReport.CreatedAt);
        }

        var endDate = DateTime.UtcNow.Date;
        var startDate = endDate.AddDays(-dto.Period);

        var priceHistoryData = await _unitOfWork.PriceHistoryData.GetByCryptoCurrencyId(cryptoCurrency.Id)
            .Where(phd => phd.Timestamp.Date >= startDate.Date)
            .OrderBy(phd => phd.Timestamp)
            .ToListAsync();

        var technicalIndicators = await _unitOfWork.TechnicalIndicators.GetByCryptoCurrencyId(cryptoCurrency.Id)
            .Where(phd => phd.Timestamp.Date >= startDate.Date)
            .ToListAsync();

        if (priceHistoryData.Count == 0 || technicalIndicators.Count == 0)
        {
            throw new HttpException("Data hasn't been uploaded", 400);
        }

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add($"Tech. Analysis - {cryptoCurrency.Name} ({cryptoCurrency.Symbol})");

        worksheet.Cell(1, 1).Value = "Date";
        worksheet.Cell(1, 2).Value = "Closing price (USD)";
        worksheet.Cell(1, 3).Value = "Indicator type";
        worksheet.Cell(1, 4).Value = "Indicator value";

        var headerRange = worksheet.Range("A1:D1");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

        int row = 2;
        foreach (var ph in priceHistoryData)
        {
            var dailyIndicators = technicalIndicators.Where(i => i.Timestamp.Date == ph.Timestamp.Date).ToList();

            if (dailyIndicators.Count != 0)
            {
                worksheet.Cell(row, 1).Value = ph.Timestamp.ToString("yyyy-MM-dd HH:mm:ss");
                worksheet.Cell(row, 2).Value = ph.PriceUsd;

                foreach (var indicator in dailyIndicators)
                {
                    worksheet.Cell(row, 3).Value = indicator.IndicatorType.ToString();
                    worksheet.Cell(row, 4).Value = indicator.Value;
                    row++;
                }
            }
        }

        worksheet.Columns().AdjustToContents();

        var excelReport = await SaveFileAsync(
            workbook, 
            $"Technical_Analisis_Report_{dto.Name}_{dto.Symbol}_{createdAt:yyyy-MM-dd}", 
            ExcelReportTypeEnum.TechnicalAnalisis, 
            createdAt, 
            cryptoCurrency.Id);

        return new GenerateReportResponseDto(excelReport.Id, excelReport.OriginalFileName, excelReport.ReportType, excelReport.CreatedAt);
    }

    public async Task<GenerateReportResponseDto> GenerateComparativeAnalysisReportAsync(GenerateComparativeAnalysisReportRequestDto dto)
    {
        if (dto.CryptoCurrencies == null || dto.CryptoCurrencies.Count == 0)
        {
            throw new HttpException("It is necessary to specify the symbols of cryptocurrencies for comparison", 400);
        }

        var cryptoCurrencies = new List<CryptoCurrency>();
        var priceHistoryDataDict = new Dictionary<Guid, List<PriceHistoryData>>();

        var endDate = DateTime.UtcNow.Date;
        var startDate = endDate.AddDays(-dto.Period);

        foreach (var cryptoCurrencyDto in dto.CryptoCurrencies)
        {
            var cryptoCurrency = await _cryptoCurrencyService.GetCryptoCurrencyWithCache(cryptoCurrencyDto.Name, cryptoCurrencyDto.Symbol);

            cryptoCurrencies.Add(cryptoCurrency);

            var priceHistoryData = await _unitOfWork.PriceHistoryData.GetByCryptoCurrencyId(cryptoCurrency.Id)
                .Where(phd => phd.Timestamp.Date >= startDate.Date)
                .OrderBy(phd => phd.Timestamp)
                .ToListAsync();

            priceHistoryDataDict.Add(cryptoCurrency.Id, priceHistoryData);
        }

        var createdAt = DateTime.UtcNow;
        var existExcelReport = await _unitOfWork.ExcelReports.GetReportAsync(cryptoCurrencies[0].Id, ExcelReportTypeEnum.ComparativeAnalysis, createdAt);
        if (existExcelReport != null)
        {
            return new GenerateReportResponseDto(existExcelReport.Id, existExcelReport.OriginalFileName, existExcelReport.ReportType, existExcelReport.CreatedAt);
        }

        if (cryptoCurrencies.Count == 0 || priceHistoryDataDict.Values.All(list => list.Count == 0))
        {
            throw new HttpException("No data available for the specified cryptocurrencies and period for comparative analysis.", 400);
        }

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Comparative Analisis");

        worksheet.Cell(1, 1).Value = "Date";
        int col = 2;
        foreach (var currency in cryptoCurrencies)
        {
            worksheet.Cell(1, col++).Value = $"{currency.Name} ({currency.Symbol}) Price (USD)";
        }

        var headerRange = worksheet.Range(1, 1, 1, col - 1);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        int row = 2;
        var allDates = priceHistoryDataDict.Values
            .SelectMany(list => list.Select(ph => ph.Timestamp.Date))
            .Distinct()
            .OrderBy(d => d)
            .ToList();

        foreach (var date in allDates)
        {
            worksheet.Cell(row, 1).Value = date.ToString("yyyy-MM-dd HH:mm:ss");
            col = 2;
            foreach (var currency in cryptoCurrencies)
            {
                var priceDataForDate = priceHistoryDataDict.GetValueOrDefault(currency.Id)?
                    .FirstOrDefault(ph => ph.Timestamp.Date == date);
                worksheet.Cell(row, col++).Value = priceDataForDate?.PriceUsd;
            }
            row++;
        }

        for (int i = 2; i < col; i++)
        {
            worksheet.Column(i).Style.NumberFormat.Format = "$#,##0.00";
        }

        worksheet.Columns().AdjustToContents();

        var excelReport = await SaveFileAsync(
            workbook,
            $"Comparative_Analysis_Report_{dto.CryptoCurrencies[0].Name}_{dto.CryptoCurrencies[0].Symbol}_{createdAt:yyyy-MM-dd}",
            ExcelReportTypeEnum.ComparativeAnalysis,
            createdAt,
            cryptoCurrencies[0].Id);

        return new GenerateReportResponseDto(excelReport.Id, excelReport.OriginalFileName, excelReport.ReportType, excelReport.CreatedAt);
    }

    public async Task<GenerateReportResponseDto> GenerateVolatilityAnalysisReportAsync(GenerateVolatilityAnalysisReportRequestDto dto)
    {
        var cryptoCurrency = await _cryptoCurrencyService.GetCryptoCurrencyWithCache(dto.Name, dto.Symbol);

        var createdAt = DateTime.UtcNow;
        var existExcelReport = await _unitOfWork.ExcelReports.GetReportAsync(cryptoCurrency.Id, ExcelReportTypeEnum.VolatilityAnalisis, createdAt);
        if (existExcelReport != null)
        {
            return new GenerateReportResponseDto(existExcelReport.Id, existExcelReport.OriginalFileName, existExcelReport.ReportType, existExcelReport.CreatedAt);
        }

        var endDate = DateTime.UtcNow.Date;
        var startDate = endDate.AddDays(-dto.Period);

        var priceHistoryData = await _unitOfWork.PriceHistoryData.GetByCryptoCurrencyId(cryptoCurrency.Id)
            .Where(phd => phd.Timestamp.Date >= startDate.Date)
            .OrderBy(phd => phd.Timestamp)
            .ToListAsync();

        var dailyReturns = new List<decimal>();
        decimal? previousPrice = null;

        foreach (var priceData in priceHistoryData.OrderBy(p => p.Timestamp))
        {
            if (priceData.PriceUsd.HasValue)
            {
                if (previousPrice.HasValue && previousPrice.Value != 0)
                {
                    var dailyReturn = (priceData.PriceUsd.Value - previousPrice.Value) / previousPrice.Value;
                    dailyReturns.Add(dailyReturn);
                }
                previousPrice = priceData.PriceUsd.Value;
            }
        }

        decimal volatility = 0m;
        if (dailyReturns.Count != 0)
        {
            var mean = dailyReturns.Average();
            var sumOfSquaresOfDifferences = dailyReturns.Sum(d => (d - mean) * (d - mean));
            volatility = (decimal)Math.Sqrt((double)(sumOfSquaresOfDifferences / dailyReturns.Count));
        }

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add($"Volatility - {cryptoCurrency.Symbol}");

        worksheet.Cell(1, 1).Value = "Cryptocurrency";
        worksheet.Cell(1, 2).Value = "Period (days)";
        worksheet.Cell(1, 3).Value = "Volatility (Standard deviation)";
        worksheet.Cell(1, 4).Value = "Average change per day (%)";

        var headerRange = worksheet.Range("A1:D1");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

        worksheet.Cell(2, 1).Value = cryptoCurrency.Symbol;
        worksheet.Cell(2, 2).Value = dto.Period;
        worksheet.Cell(2, 3).Value = volatility;
        worksheet.Cell(2, 4).Value = dailyReturns.Count != 0 ? dailyReturns.Average() : 0m;

        worksheet.Column(3).Style.NumberFormat.Format = "0.00\\%"; 
        worksheet.Column(4).Style.NumberFormat.Format = "0.00\\%";

        worksheet.Columns().AdjustToContents();

        var excelReport = await SaveFileAsync(
            workbook,
            $"Volatility_Report_{dto.Name}_{dto.Symbol}_{createdAt:yyyy-MM-dd}",
            ExcelReportTypeEnum.VolatilityAnalisis,
            createdAt,
            cryptoCurrency.Id);

        return new GenerateReportResponseDto(excelReport.Id, excelReport.OriginalFileName, excelReport.ReportType, excelReport.CreatedAt);
    }

    public async Task<DownloadReportResponseDto> DownloadReportAsync(DownloadReportRequestDto dto)
    {
        var excelReport = await _unitOfWork.ExcelReports.GetByIdAsync(dto.Id)
            ?? throw new HttpException("ExcelReport not founds", 400);

        return new DownloadReportResponseDto(new FileStream(excelReport.FilePath, FileMode.Open, FileAccess.Read), excelReport.OriginalFileName);
    }

    private async Task<ExcelReport> SaveFileAsync(
        XLWorkbook workbook, 
        string originalFileName, 
        ExcelReportTypeEnum reportType,
        DateTime createdAt, 
        Guid cryptoCurrencyId)
    {
        if (!Directory.Exists(_reportsStoragePath))
        {
            Directory.CreateDirectory(_reportsStoragePath);
        }

        var id = Guid.NewGuid();
        var fileType = "xlsx";
        var filePath = Path.Combine(_reportsStoragePath, $"{id}.{fileType}");

        var request = _httpContextAccessor.HttpContext?.Request;
        var url = $"{request?.Scheme}://{request?.Host}/reports/{id}.{fileType}";

        var excelReport = new ExcelReport
        {
            Id = id,
            FilePath = filePath,
            FileUrl = url,
            FileType = fileType,
            OriginalFileName = originalFileName,
            ReportType = reportType,
            CreatedAt = createdAt,
            CryptoCurrencyId = cryptoCurrencyId
        };

        workbook.SaveAs(filePath);

        await _unitOfWork.ExcelReports.AddAsync(excelReport);
        await _unitOfWork.SaveAsync();

        return excelReport;
    }
}