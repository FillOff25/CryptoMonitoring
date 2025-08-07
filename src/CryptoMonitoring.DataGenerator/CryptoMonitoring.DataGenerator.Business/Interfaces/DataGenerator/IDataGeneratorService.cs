using CryptoMonitoring.Common.DTOs.DataGenerator;
using CryptoMonitoring.Models.Entities;

namespace CryptoMonitoring.DataGenerator.Business.Interfaces.DataGenerator;

public interface IDataGeneratorService
{
    Task<GenerateDataRabbitMQResponseDto<List<MarketData>>> GenerateMarketDataAsync(GenerateDataRequestDto dto);
    Task<GenerateDataRabbitMQResponseDto<List<PriceHistoryData>>> GeneratePriceHistoryDataAsync(GenerateDataRequestDto dto);
}