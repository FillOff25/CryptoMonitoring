using CryptoMonitoring.Common.DTOs.DataGenerator;
using CryptoMonitoring.Models.Entities;

namespace CryptoMonitoring.DataProcessor.Business.Interfaces.Processing;

public interface ISyntheticDataProcessingService
{
    Task<GenerateDataResponseDto> ProcessPriceHistoryDataAsync(GenerateDataRabbitMQResponseDto<List<PriceHistoryData>> dto);
    Task<GenerateDataResponseDto> ProcessSyntheticMarketDataAsync(GenerateDataRabbitMQResponseDto<List<MarketData>> dto);
}