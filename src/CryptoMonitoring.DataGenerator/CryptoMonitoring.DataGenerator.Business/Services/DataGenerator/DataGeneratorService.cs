using CryptoMonitoring.Common.DTOs.DataGenerator;
using CryptoMonitoring.DataGenerator.Business.Interfaces.DataGenerator;
using CryptoMonitoring.Common.Persistence.Interfaces;
using CryptoMonitoring.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;
using CryptoMonitoring.Common.Exceptions;

namespace CryptoMonitoring.DataGenerator.Business.Services.DataGenerator;

public class DataGeneratorService : IDataGeneratorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Random _random;

    public DataGeneratorService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _random = new Random();
    }

    public async Task<GenerateDataRabbitMQResponseDto<List<MarketData>>> GenerateMarketDataAsync(GenerateDataRequestDto dto)
    {
        var cryptoCurrency = await _unitOfWork.CryptoCurrencies.GetByNameAndSymbolAsync(dto.Name, dto.Symbol)
            ?? throw new HttpException($"Cryptocurrency (name: {dto.Name}, symbol: {dto.Symbol}) does not exist in database", 400);

        var generatedData = new List<MarketData>();
        var existingMarketData = await _unitOfWork.MarketData.GetByCryptoCurrencyId(cryptoCurrency.Id).ToListAsync();

        decimal baseVolume24hUsd = GetBaseValue(existingMarketData, md => md.Volume24hUsd, _random, 1000m, 2000m);
        decimal baseVwap24hUsd = GetBaseValue(existingMarketData, md => md.Vwap24hUsd, _random, 1000m, 2000m);
        decimal baseCirculatingSupply = GetBaseValue(existingMarketData, md => md.CirculatingSupply, _random, 1000m, 2000m);

        decimal lastVwap24hUsd = baseVwap24hUsd;
        decimal lastVolume24hUsd = baseVolume24hUsd;
        decimal lastMarketCapUsd = 0;
        decimal lastCirculatingSupply = baseCirculatingSupply;

        DateTime currentTimestamp = DateTime.UtcNow;

        for (int i = 0; i < dto.Count; i++)
        {
            decimal volume24hUsd = baseVolume24hUsd * (1m + ((decimal)(_random.NextDouble() * 0.2) - 0.1m));
            decimal currentVwap24hUsd = baseVwap24hUsd * 1m + ((decimal)(_random.NextDouble() * 0.2) - 0.1m);
            decimal circulatingSupply = baseCirculatingSupply * (1m + ((decimal)(_random.NextDouble() * 0.2) - 0.1m));

            decimal marketCapUsd = currentVwap24hUsd * circulatingSupply;
            decimal change24hPercent = (decimal)(_random.NextDouble() * 30) - 15;

            var marketData = new MarketData
            {
                Id = Guid.NewGuid(),
                Timestamp = currentTimestamp,
                Volume24hUsd = volume24hUsd,
                MarketCapUsd = marketCapUsd,
                Vwap24hUsd = currentVwap24hUsd,
                Change24hPercent = change24hPercent,
                CirculatingSupply = circulatingSupply,
                CryptoCurrencyId = cryptoCurrency.Id
            };

            generatedData.Add(marketData);

            lastVwap24hUsd = currentVwap24hUsd;
            lastVolume24hUsd = volume24hUsd;
            lastMarketCapUsd = marketCapUsd;
            lastCirculatingSupply = circulatingSupply;

            currentTimestamp = currentTimestamp.AddDays(-1);
        }

        Log.Information($"Successfully generated {generatedData.Count} synthetic market data for {dto.Name} ({dto.Symbol})");
        return new GenerateDataRabbitMQResponseDto<List<MarketData>>(dto.Name, dto.Symbol, generatedData);
    }

    public async Task<GenerateDataRabbitMQResponseDto<List<PriceHistoryData>>> GeneratePriceHistoryDataAsync(GenerateDataRequestDto dto)
    {
        var cryptoCurrency = await _unitOfWork.CryptoCurrencies.GetByNameAndSymbolAsync(dto.Name, dto.Symbol) 
            ?? throw new HttpException($"Cryptocurrency {dto.Name} ({dto.Symbol}) does not exist in database", 400);
        
        var generatedData = new List<PriceHistoryData>();
        var existingPriceHistoryData = await _unitOfWork.PriceHistoryData.GetByCryptoCurrencyId(cryptoCurrency.Id).ToListAsync();

        decimal basePriceUsd = GetBaseValue(existingPriceHistoryData, phd => phd.PriceUsd, _random, 1000m, 2000m);
        decimal lastPriceUsd = basePriceUsd;

        DateTime currentTimestamp = DateTime.UtcNow;

        for (int i = 0; i < dto.Count; i++)
        {
            decimal priceUsd = basePriceUsd * (1m + ((decimal)(_random.NextDouble() * 0.2) - 0.1m));

            var priceHistoryData = new PriceHistoryData
            {
                Id = Guid.NewGuid(),
                Timestamp = currentTimestamp,
                PriceUsd = priceUsd,
                CryptoCurrencyId = cryptoCurrency.Id
            };

            generatedData.Add(priceHistoryData);

            lastPriceUsd = priceUsd;
            currentTimestamp = currentTimestamp.AddDays(-1);
        }

        Log.Information($"Successfully generated {generatedData.Count} synthetic price history data for {dto.Name} ({dto.Symbol})");
        return new GenerateDataRabbitMQResponseDto<List<PriceHistoryData>>(dto.Name, dto.Symbol, generatedData);
    }

    private decimal GetBaseValue<TData>(
        List<TData> priceHistoryData,
        Func<TData, decimal?> selector,
        Random random,
        decimal minValue,
        decimal maxValue)
    {
        var validValues = priceHistoryData.Select(selector).Where(v => v.HasValue).Select(v => v!.Value).ToList();

        if (validValues.Count != 0)
        {
            decimal avg = validValues.Average();
            return Math.Max(minValue, Math.Min(maxValue, avg));
        }
        else
        {
            return minValue + (decimal)(random.NextDouble() * (double)(maxValue - minValue));
        }
    }
}
