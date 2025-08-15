using ClosedXML.Excel;
using CryptoMonitoring.Common.DTOs.ReportGenerator;
using CryptoMonitoring.Common.Exceptions;
using CryptoMonitoring.Common.Interfaces.Caching;
using CryptoMonitoring.Common.Persistence.Interfaces;
using CryptoMonitoring.Models.Entities;
using CryptoMonitoring.ReportGenerator.Business.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CryptoMonitoring.ReportGenerator.Business.Services;

public class ExcelReportService : IExcelReportService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisCacherService _redisCacherService;

    public ExcelReportService(
        IUnitOfWork unitOfWork, 
        IRedisCacherService redisCacherService)
    {
        _unitOfWork = unitOfWork;
        _redisCacherService = redisCacherService;
    }

    public async Task<MemoryStream> GenerateDailyReportAsync(GenerateDailyReportRequestDto dto)
    {
        var cryptoCurrency = await _redisCacherService.GetAsync<CryptoCurrency>(_redisCacherService.GenerateCryptoCurrencyKey(dto.Name, dto.Symbol));

        if (cryptoCurrency == null)
        {
            cryptoCurrency = await _unitOfWork.CryptoCurrencies.GetByNameAndSymbolAsync(dto.Name, dto.Symbol)
                ?? throw new HttpException($"Cryptocurrency (name: {dto.Name}, symbol: {dto.Symbol}) does not exist in database", 400);

            await _redisCacherService.SetAsync(_redisCacherService.GenerateCryptoCurrencyKey(dto.Name, dto.Symbol), cryptoCurrency);
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

            worksheet.Cell(row, 1).Value = ph.Timestamp.ToString("yyyy-MM-dd");
            worksheet.Cell(row, 2).Value = $"{cryptoCurrency.Name} ({cryptoCurrency.Symbol})";
            worksheet.Cell(row, 3).Value = ph.PriceUsd;
            worksheet.Cell(row, 4).Value = md?.Volume24hUsd;
            worksheet.Cell(row, 5).Value = md?.Change24hPercent;
            worksheet.Cell(row, 6).Value = md?.MarketCapUsd;
            row++;
        }

        worksheet.Column(1).Style.DateFormat.SetFormat("yyyy-MM-dd");
        worksheet.Column(3).Style.NumberFormat.Format = "$#,##0.00";
        worksheet.Column(4).Style.NumberFormat.Format = "#,##0";
        worksheet.Column(5).Style.NumberFormat.Format = "0.000\\%";
        worksheet.Column(6).Style.NumberFormat.Format = "$#,##0";

        worksheet.Columns().AdjustToContents();

        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Seek(0, SeekOrigin.Begin);

        return stream;
    }

    public async Task<MemoryStream> GenerateTechnicalAnalysisReportAsync(GenerateTechnicalAnalysisReportRequestDto dto)
    {
        var cryptoCurrency = await _redisCacherService.GetAsync<CryptoCurrency>(_redisCacherService.GenerateCryptoCurrencyKey(dto.Name, dto.Symbol));

        if (cryptoCurrency == null)
        {
            cryptoCurrency = await _unitOfWork.CryptoCurrencies.GetByNameAndSymbolAsync(dto.Name, dto.Symbol)
                ?? throw new HttpException($"Cryptocurrency (name: {dto.Name}, symbol: {dto.Symbol}) does not exist in database", 400);

            await _redisCacherService.SetAsync(_redisCacherService.GenerateCryptoCurrencyKey(dto.Name, dto.Symbol), cryptoCurrency);
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
                worksheet.Cell(row, 1).Value = ph.Timestamp.ToString("yyyy-MM-dd");
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

        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Seek(0, SeekOrigin.Begin);

        return stream;
    }
}