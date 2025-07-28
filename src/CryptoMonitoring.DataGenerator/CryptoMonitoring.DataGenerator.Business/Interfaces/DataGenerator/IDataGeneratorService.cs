using CryptoMonitoring.DataGenerator.Business.DTOs.DataGenerator;

namespace CryptoMonitoring.DataGenerator.Business.Interfaces.DataGenerator;

public interface IDataGeneratorService
{
    Task GenerateMarketDataAsync(GenerateDataRequestDto dto);
    Task GeneratePriceHistoryDataAsync(GenerateDataRequestDto dto);
}