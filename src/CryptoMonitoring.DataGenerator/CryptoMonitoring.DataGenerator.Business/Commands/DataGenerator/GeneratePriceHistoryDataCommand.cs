using CryptoMonitoring.Common.DTOs.DataGenerator;
using CryptoMonitoring.Common.Extensions;
using CryptoMonitoring.Common.Interfaces;
using CryptoMonitoring.DataGenerator.Business.Interfaces.DataGenerator;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitoring.DataGenerator.Business.Commands.DataGenerator;

public class GeneratePriceHistoryDataCommand : ICommand<GenerateDataRequestDto, IActionResult>
{
    private readonly IDataGeneratorService _dataGeneratorService;

    public GeneratePriceHistoryDataCommand(IDataGeneratorService dataGeneratorService)
    {
        _dataGeneratorService = dataGeneratorService;
    }

    public async Task<IActionResult> ExecuteAsync(GenerateDataRequestDto dto)
    {
        return (await _dataGeneratorService.GenerateMarketDataAsync(dto))
            .ToHttpResponse("Price history data was generated successfully", 201);
    }
}
