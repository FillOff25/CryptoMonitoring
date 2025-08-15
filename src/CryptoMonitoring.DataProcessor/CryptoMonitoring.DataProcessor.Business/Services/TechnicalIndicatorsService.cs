using CryptoMonitoring.Common.DTOs.DataProcessor;
using CryptoMonitoring.Common.Exceptions;
using CryptoMonitoring.Common.Interfaces.Caching;
using CryptoMonitoring.Common.Persistence.Interfaces;
using CryptoMonitoring.DataProcessor.Business.Interfaces;
using CryptoMonitoring.Models.Entities;
using CryptoMonitoring.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace CryptoMonitoring.DataProcessor.Business.Services;

public class TechnicalIndicatorsService : ITechnicalIndicatorsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisCacherService _redisCacherService;

    public TechnicalIndicatorsService(
        IUnitOfWork unitOfWork, 
        IRedisCacherService redisCacherService)
    {
        _unitOfWork = unitOfWork;
        _redisCacherService = redisCacherService;
    }

    public async Task CalculateTechnicalIndicatorAsync(CalculateTechnicalIndicatorRequestDto dto)
    {
        var cryptoCurrency = await _redisCacherService.GetAsync<CryptoCurrency>(_redisCacherService.GenerateCryptoCurrencyKey(dto.Name, dto.Symbol));

        if (cryptoCurrency == null)
        {
            cryptoCurrency = await _unitOfWork.CryptoCurrencies.GetByNameAndSymbolAsync(dto.Name, dto.Symbol)
                ?? throw new HttpException($"Cryptocurrency (name: {dto.Name}, symbol: {dto.Symbol}) does not exist in database", 400);

            await _redisCacherService.SetAsync(_redisCacherService.GenerateCryptoCurrencyKey(dto.Name, dto.Symbol), cryptoCurrency);
        }

        decimal? value = 0;

        switch (dto.Type)
        {
            case IndicatorTypeEnum.SMA:
                value = await CalculateSMAValueAsync(cryptoCurrency.Id);
                break;
            case IndicatorTypeEnum.EMA:
                value = await CalculateEMAValueAsync(cryptoCurrency.Id);
                break;
            case IndicatorTypeEnum.RSI:
                value = await CalculateRSIValueAsync(cryptoCurrency.Id);
                break;
        }

        var timestamp = DateTime.UtcNow;
        if (!await _unitOfWork.TechnicalIndicators.IsExist(cryptoCurrency.Id, dto.Type, timestamp))
        {
            var indicator = new TechnicalIndicator
            {
                Id = Guid.NewGuid(),
                Timestamp = timestamp,
                IndicatorType = dto.Type,
                Value = value ?? 0,
                CryptoCurrencyId = cryptoCurrency.Id,
            };

            await _unitOfWork.TechnicalIndicators.AddAsync(indicator);
            await _unitOfWork.SaveAsync();
        }
    }

    private async Task<decimal?> CalculateSMAValueAsync(Guid cryptoCurrencyId)
    {
        int period = 20;
        var priceHistoryData = await _unitOfWork.PriceHistoryData
            .GetByCryptoCurrencyId(cryptoCurrencyId)
            .OrderByDescending(phd => phd.Timestamp)
            .Take(period)
            .ToListAsync();

        if (priceHistoryData.Count < period)
        {
            throw new HttpException("There are not enough price history data", 400);
        }

        var value = priceHistoryData.Average(phd => phd.PriceUsd);

        return value;
    }

    private async Task<decimal?> CalculateEMAValueAsync(Guid cryptoCurrencyId)
    {
        int period = 20;
        var priceHistoryData = await _unitOfWork.PriceHistoryData
            .GetByCryptoCurrencyId(cryptoCurrencyId)
            .OrderBy(phd => phd.Timestamp)
            .ToListAsync();

        if (priceHistoryData.Count < period)
        {
            throw new HttpException("There are not enough price history data", 400);
        }

        decimal smoothingFactor = 2m / (period + 1);

        var initialSMA = priceHistoryData
            .Take(period)
            .Average(phd => phd.PriceUsd);

        var currentEMA = initialSMA;
        for (int i = period; i < priceHistoryData.Count; i++)
        {
            var currentPrice = priceHistoryData[i].PriceUsd;
            currentEMA = (currentPrice - currentEMA) * smoothingFactor + currentEMA;
        }

        return currentEMA;
    }

    private async Task<decimal?> CalculateRSIValueAsync(Guid cryptoCurrencyId)
    {
        const int period = 14;

        var priceHistoryData = await _unitOfWork.PriceHistoryData
            .GetByCryptoCurrencyId(cryptoCurrencyId)
            .OrderBy(phd => phd.Timestamp)
            .Take(period + 1)
            .ToListAsync();

        if (priceHistoryData.Count < period + 1)
        {
            throw new HttpException("There are not enough price history data", 400);
        }

        var gains = new List<decimal>();
        var losses = new List<decimal>();

        for (int i = 1; i <= period; i++)
        {
            var priceChange = priceHistoryData[i].PriceUsd - priceHistoryData[i - 1].PriceUsd;

            if (priceChange > 0)
            {
                gains.Add(priceChange.Value);
                losses.Add(0);
            }
            else
            {
                gains.Add(0);
                losses.Add(priceChange!.Value * -1);
            }
        }

        var averageGain = gains.Average();
        var averageLoss = losses.Average();

        decimal rs;
        if (averageLoss == 0)
        {
            rs = 100;
        }
        else
        {
            rs = averageGain / averageLoss;
        }

        var rsi = 100 - (100 / (1 + rs));

        return rsi;
    }
}