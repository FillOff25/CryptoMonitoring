using AutoMapper;
using CryptoMonitoring.Common.DTOs.CoinGeckoApi;
using CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;
using CryptoMonitoring.DataGenerator.Persistence.Interfaces;
using CryptoMonitoring.Models.Entities;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace CryptoMonitoring.DataGenerator.Business.Services.ExternalApis;

public class CoinGeckoApiService : ICoinGeckoApiService
{
    private readonly IExternalApiHttpClient _externalApiHttpClient;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CoinGeckoApiService(
        IExternalApiHttpClient externalApiHttpClient,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IConfiguration configuration)
    {
        _externalApiHttpClient = externalApiHttpClient;
        _externalApiHttpClient.SetBaseAddress(configuration["COINGECKO_BASE_URL_ADDRESS"]!);
        _externalApiHttpClient.SetHeaderApiKey("x-cg-demo-api-key", configuration["COINGECKO_API_KEY"]!);

        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetCoinGeckoDataResponseDto> GetCryptoCurrencyAsync()
    {
        try
        {
            var entities = await _externalApiHttpClient.GetDataAsync<List<CoinGeckoCoinResponseDto>>("coins/list");

            if (entities == null)
            {
                return new GetCoinGeckoDataResponseDto(0);
            }

            var newCryptoCurrencies = _mapper.Map<List<CryptoCurrency>>(entities);
            var processedNamesAndSymbols = new HashSet<(string Name, string Symbol)>();
            var addedAndUpdatedCount = 0;

            foreach (var newCryptoCurrency in newCryptoCurrencies)
            {
                var currentKey = (newCryptoCurrency.Name, newCryptoCurrency.Symbol);

                if (processedNamesAndSymbols.Contains(currentKey))
                {
                    continue;
                }

                if (!await _unitOfWork.CryptoCurrencies.IsCoinGeckoIdExistAsync(newCryptoCurrency.CoinGeckoId!))
                {
                    if (await _unitOfWork.CryptoCurrencies.IsNameAndSymbolExistAsync(newCryptoCurrency.Name, newCryptoCurrency.Symbol))
                    {
                        var cryptoCurrency = await _unitOfWork.CryptoCurrencies.GetByNameAndSymbolAsync(newCryptoCurrency.Name, newCryptoCurrency.Symbol);
                        cryptoCurrency!.CoinGeckoId = newCryptoCurrency.CoinGeckoId;

                        _unitOfWork.CryptoCurrencies.Update(cryptoCurrency);
                    }
                    else
                    {
                        newCryptoCurrency.Id = Guid.NewGuid();
                        await _unitOfWork.CryptoCurrencies.AddAsync(newCryptoCurrency);
                    }

                    addedAndUpdatedCount++;
                }

                processedNamesAndSymbols.Add(currentKey);
            }

            if (addedAndUpdatedCount > 0)
            {
                await _unitOfWork.SaveAsync();

                Log.Information($"Successfully retrieved cryptocurrency data from CoinGecko Api. {addedAndUpdatedCount} cryptocurrencies were added or updated to the database");
            }
            else
            {
                Log.Information("Successfully retrieved cryptocurrency data from CoinGecko Api, but no new cryptocurrencies were added as all already existed");
            }

            return new GetCoinGeckoDataResponseDto(addedAndUpdatedCount);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while getting cryptocurrency data from CoinGecko Api");
            throw;
        }
    }

    public async Task<GetCoinGeckoMarketDataByIdResponseDto> GetMarketDataByIdAsync(CoinGeckoCoinWithMarketDataRequestDto dto)
    {
        try
        {
            var cryptoCurrency = (await _unitOfWork.CryptoCurrencies.GetByCoinGeckoIdAsync(dto.CoinGeckoId))!;

            if (cryptoCurrency == null)
            {
                return new GetCoinGeckoMarketDataByIdResponseDto(string.Empty, string.Empty, DateTime.UtcNow);
            }

            var entity = await _externalApiHttpClient.GetDataAsync<List<CoinGeckoCoinWithMarketDataResponseDto>>(
                $"coins/markets?vs_currency=usd&ids={dto.CoinGeckoId}");

            if (entity == null)
            {
                return new GetCoinGeckoMarketDataByIdResponseDto(string.Empty, string.Empty, DateTime.UtcNow);
            }

            var newMarketData = _mapper.Map<List<MarketData>>(entity);

            newMarketData[0].CryptoCurrencyId = cryptoCurrency.Id;

            if (!await _unitOfWork.MarketData.IsUpdatedTodayAsync(newMarketData[0].CryptoCurrencyId, newMarketData[0].Timestamp))
            {
                newMarketData[0].Id = Guid.NewGuid();
                newMarketData[0].CryptoCurrencyId = cryptoCurrency.Id;

                await _unitOfWork.MarketData.AddAsync(newMarketData[0]);
                await _unitOfWork.SaveAsync();

                Log.Information($"Market data successfully retrieved and changes applied to the database from CoinGecko Api");
            }

            return new GetCoinGeckoMarketDataByIdResponseDto(cryptoCurrency.Name, cryptoCurrency.Symbol, newMarketData[0].Timestamp);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while getting market data from CoinGecko Api");
            throw;
        }
    }

    public async Task<GetCoinGeckoPriceHistoryDataByIdResponseDto> GetPriceHistoryDataByIdAsync(CoinGeckoHistoricalChartDataRequestDto dto)
    {
        try
        {
            var cryptoCurrency = await _unitOfWork.CryptoCurrencies.GetByCoinGeckoIdAsync(dto.CoinGeckoId);

            if (cryptoCurrency == null)
            {
                return new GetCoinGeckoPriceHistoryDataByIdResponseDto(string.Empty, string.Empty, 0);
            }

            var entity = await _externalApiHttpClient.GetDataAsync<CoinGeckoHistoricalChartDataResponseDto>(
                $"coins/{dto.CoinGeckoId}/market_chart?vs_currency=usd&days=100");

            if (entity == null)
            {
                return new GetCoinGeckoPriceHistoryDataByIdResponseDto(string.Empty, string.Empty, 0);
            }

            var priceHistoryData = _mapper.Map<List<PriceHistoryData>>(entity.Prices);
            var addedCount = 0;

            foreach (var priceHistory in priceHistoryData)
            {
                if (!await _unitOfWork.PriceHistoryData.IsExist(cryptoCurrency!.Id, priceHistory.Timestamp))
                {
                    priceHistory.Id = Guid.NewGuid();
                    priceHistory.CryptoCurrencyId = cryptoCurrency.Id;

                    await _unitOfWork.PriceHistoryData.AddAsync(priceHistory);
                    addedCount++;
                }
            }

            if (addedCount > 0)
            {
                await _unitOfWork.SaveAsync();

                Log.Information($"Successfully retrieved price history data from CoinGecko Api");
            }

            return new GetCoinGeckoPriceHistoryDataByIdResponseDto(cryptoCurrency.Name, cryptoCurrency.Symbol, addedCount);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while getting price history data from CoinGecko Api");
            throw;
        }
    }
}
