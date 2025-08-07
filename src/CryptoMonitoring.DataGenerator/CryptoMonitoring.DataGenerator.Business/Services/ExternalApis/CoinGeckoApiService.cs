using CryptoMonitoring.Common.DTOs.CoinGeckoApi;
using CryptoMonitoring.Common.Exceptions;
using CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;
using CryptoMonitoring.Common.Persistence.Interfaces;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace CryptoMonitoring.DataGenerator.Business.Services.ExternalApis;

public class CoinGeckoApiService : ICoinGeckoApiService
{
    private readonly IExternalApiHttpClient _externalApiHttpClient;
    private readonly IUnitOfWork _unitOfWork;

    public CoinGeckoApiService(
        IExternalApiHttpClient externalApiHttpClient,
        IUnitOfWork unitOfWork,
        IConfiguration configuration)
    {
        _externalApiHttpClient = externalApiHttpClient;
        _externalApiHttpClient.SetBaseAddress(configuration["COINGECKO_BASE_URL_ADDRESS"]!);
        _externalApiHttpClient.SetHeaderApiKey("x-cg-demo-api-key", configuration["COINGECKO_API_KEY"]!);

        _unitOfWork = unitOfWork;
    }

    public async Task<List<CoinGeckoCoinResponseDto>> GetCryptoCurrencyAsync()
    {
        var entities = await _externalApiHttpClient.GetDataAsync<List<CoinGeckoCoinResponseDto>>("coins/list") 
            ?? throw new HttpException("No cryptocurrency data fetched from CoinGecko Api", 400);

        Log.Information("Cryptocurrency data fetched successfully from CoinGecko");
        return entities;
    }

    public async Task<CoinGeckoMarketDataByIdRabbitMQResponseDto> GetMarketDataByIdAsync(CoinGeckoCoinWithMarketDataRequestDto dto)
    {
        var cryptoCurrency = await _unitOfWork.CryptoCurrencies.GetByCoinGeckoIdAsync(dto.CoinGeckoId) 
            ?? throw new HttpException($"Cryptocurrency {dto.CoinGeckoId} does not exist in database", 400);

        var entity = await _externalApiHttpClient.GetDataAsync<List<CoinGeckoCoinWithMarketDataResponseDto>>($"coins/markets?vs_currency=usd&ids={dto.CoinGeckoId}") 
            ?? throw new HttpException($"No market data fetched from CoinGecko Api", 400);

        Log.Information("Market data fetched successfully from CoinGecko");
        return new CoinGeckoMarketDataByIdRabbitMQResponseDto(dto.CoinGeckoId, entity);
    }

    public async Task<CoinGeckoPriceHistoryDataByIdRabbitMQResponseDto> GetPriceHistoryDataByIdAsync(CoinGeckoHistoricalChartDataRequestDto dto)
    {
        var cryptoCurrency = await _unitOfWork.CryptoCurrencies.GetByCoinGeckoIdAsync(dto.CoinGeckoId) 
            ?? throw new HttpException($"Cryptocurrency {dto.CoinGeckoId} does not exist in database", 400);
        
        var entity = await _externalApiHttpClient.GetDataAsync<CoinGeckoHistoricalChartDataResponseDto>($"coins/{dto.CoinGeckoId}/market_chart?vs_currency=usd&days=100")
            ?? throw new HttpException($"No market data fetched from CoinGecko Api", 400);

        Log.Information("Price history data fetched successfully from CoinGecko");
        return new CoinGeckoPriceHistoryDataByIdRabbitMQResponseDto(dto.CoinGeckoId, entity);
    }
}
