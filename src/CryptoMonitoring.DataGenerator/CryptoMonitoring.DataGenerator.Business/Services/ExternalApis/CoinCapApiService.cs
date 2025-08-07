using CryptoMonitoring.Common.DTOs.CoinCapApi;
using CryptoMonitoring.Common.Exceptions;
using CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;
using CryptoMonitoring.Common.Persistence.Interfaces;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace CryptoMonitoring.DataGenerator.Business.Services.ExternalApis;

public class CoinCapApiService : ICoinCapApiService
{
    private readonly IExternalApiHttpClient _externalApiHttpClient;
    private readonly IUnitOfWork _unitOfWork;

    public CoinCapApiService(
        IExternalApiHttpClient externalApiHttpClient,
        IConfiguration configuration,
        IUnitOfWork unitOfWork)
    {
        _externalApiHttpClient = externalApiHttpClient;
        _externalApiHttpClient.SetBaseAddress(configuration["COINCAP_BASE_URL_ADDRESS"]!);
        _externalApiHttpClient.SetHeaderApiKey("Authorization", $"Bearer {configuration["COINCAP_API_KEY"]!}");

        _unitOfWork = unitOfWork;
    }

    public async Task<CoinCapResponseDto<List<CoinCapAssetResponseDto>>> GetCryptoCurrencyAsync()
    {
        var entities = await _externalApiHttpClient.GetDataAsync<CoinCapResponseDto<List<CoinCapAssetResponseDto>>>("assets");

        if (entities == null || entities?.Data == null || entities.Data.Count == 0)
        {
            throw new HttpException("No cryptocurrency data fetched from CoinCap Api", 400);
        }

        Log.Information("Cryptocurrency data fetched successfully from CoinCap");
        return entities;
    }

    public async Task<CoinCapResponseDto<List<CoinCapAssetResponseDto>>> GetMarketDataAsync()
    {
        var entities = await _externalApiHttpClient.GetDataAsync<CoinCapResponseDto<List<CoinCapAssetResponseDto>>>("assets");

        if (entities == null || entities?.Data == null || entities.Data.Count == 0)
        {
            throw new HttpException("No market data fetched from CoinCap Api", 400);
        }

        Log.Information("Market data fetched successfully from CoinCap");
        return entities;
    }

    public async Task<CoinCapPriceHistoryDataRabbitMQResponseDto> GetPriceHistoryByIdAsync(CoinCapHistoryDataRequestDto dto)
    {
        var cryptoCurrency = await _unitOfWork.CryptoCurrencies.GetByCoinCapIdAsync(dto.CoinCapId) 
            ?? throw new HttpException($"Cryptocurrency {dto.CoinCapId} does not exist in database", 400);

        var entities = await _externalApiHttpClient.GetDataAsync<CoinCapResponseDto<List<CoinCapHistoryDataResponseDto>>>(
            $"/v3/assets/{dto.CoinCapId}/history?interval=d1");

        if (entities == null || entities?.Data == null || entities.Data.Count == 0)
        {
            throw new HttpException("No price history data fetched from CoinCap Api", 400);
        }

        Log.Information("Price history data fetched successfully from CoinCap");
        return new CoinCapPriceHistoryDataRabbitMQResponseDto(dto.CoinCapId, entities);
    }
}