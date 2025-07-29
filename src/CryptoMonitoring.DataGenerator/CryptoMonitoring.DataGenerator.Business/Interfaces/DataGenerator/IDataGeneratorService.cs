using CryptoMonitoring.Common.DTOs.DataGenerator;

namespace CryptoMonitoring.DataGenerator.Business.Interfaces.DataGenerator;

public interface IDataGeneratorService
{
    Task<GenerateDataResponseDto> GenerateMarketDataAsync(GenerateDataRequestDto dto);
    Task<GenerateDataResponseDto> GeneratePriceHistoryDataAsync(GenerateDataRequestDto dto);
}