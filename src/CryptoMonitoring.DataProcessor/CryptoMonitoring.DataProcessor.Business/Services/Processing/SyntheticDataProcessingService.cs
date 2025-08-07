using CryptoMonitoring.Common.DTOs.DataGenerator;
using CryptoMonitoring.Common.Persistence.Interfaces;
using CryptoMonitoring.DataProcessor.Business.Interfaces.Processing;
using CryptoMonitoring.Models.Entities;
using Serilog;

namespace CryptoMonitoring.DataProcessor.Business.Services.Processing;

public class SyntheticDataProcessingService : ISyntheticDataProcessingService
{
    private readonly IUnitOfWork _unitOfWork;

    public SyntheticDataProcessingService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<GenerateDataResponseDto> ProcessSyntheticMarketDataAsync(GenerateDataRabbitMQResponseDto<List<MarketData>> dto)
    {
        var cryptoCurrency = (await _unitOfWork.CryptoCurrencies.GetByNameAndSymbolAsync(dto.Name, dto.Symbol))!;

        var processedData = new List<MarketData>();

        foreach (var data in dto.Data)
        {
            if (!await _unitOfWork.MarketData.IsUpdatedTodayAsync(cryptoCurrency.Id, data.Timestamp))
            {
                processedData.Add(data);
            }
        }

        if (processedData.Count != 0)
        {
            foreach (var data in processedData)
            {
                await _unitOfWork.MarketData.AddAsync(data);
            }

            await _unitOfWork.SaveAsync();
            Log.Information($"Successfully saved {processedData.Count} synthetic market data for {dto.Name} ({dto.Symbol})");
        }
        else
        {
            Log.Information($"No new synthetic market data saved for {dto.Name} ({dto.Symbol}). All days already existed");
        }

        return new GenerateDataResponseDto(dto.Name, dto.Symbol, processedData.Count);
    }

    public async Task<GenerateDataResponseDto> ProcessPriceHistoryDataAsync(GenerateDataRabbitMQResponseDto<List<PriceHistoryData>> dto)
    {
        var cryptoCurrency = (await _unitOfWork.CryptoCurrencies.GetByNameAndSymbolAsync(dto.Name, dto.Symbol))!;

        var processedData = new List<PriceHistoryData>();

        foreach (var data in dto.Data)
        {
            if (!await _unitOfWork.PriceHistoryData.IsExist(cryptoCurrency.Id, data.Timestamp))
            {
                processedData.Add(data);
            }
        }

        if (processedData.Count != 0)
        {
            foreach (var data in processedData)
            {
                await _unitOfWork.PriceHistoryData.AddAsync(data);
            }

            await _unitOfWork.SaveAsync();
            Log.Information($"Successfully saved {processedData.Count} synthetic price history data for {dto.Name} ({dto.Symbol})");
        }
        else
        {
            Log.Information($"No new synthetic price history saved for {dto.Name} ({dto.Symbol}). All days already existed");
        }

        return new GenerateDataResponseDto(dto.Name, dto.Symbol, processedData.Count);
    }
}
