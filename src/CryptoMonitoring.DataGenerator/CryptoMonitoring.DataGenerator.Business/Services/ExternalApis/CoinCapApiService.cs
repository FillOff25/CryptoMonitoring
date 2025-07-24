using AutoMapper;
using CryptoMonitoring.DataGenerator.Business.DTOs.CoinCapApi;
using CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;
using CryptoMonitoring.DataGenerator.Persistence.Interfaces;
using CryptoMonitoring.Models.Entities;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace CryptoMonitoring.DataGenerator.Business.Services.ExternalApis;

public class CoinCapApiService : ICoinCapApiService
{
    private readonly IExternalApiHttpClient _externalApiHttpClient;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CoinCapApiService(
        IExternalApiHttpClient externalApiHttpClient,
        IConfiguration configuration,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _externalApiHttpClient = externalApiHttpClient;
        _externalApiHttpClient.SetBaseAddress(configuration["COINCAP_BASE_URL_ADDRESS"]!);
        _externalApiHttpClient.SetHeaderApiKey("Authorization", $"Bearer {configuration["COINCAP_API_KEY"]!}");

        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task GetCryptoCurrencyAsync()
    {
        try
        {
            var entities = await _externalApiHttpClient.GetDataAsync<CoinCapResponseDto<List<CoinCapAssetResponseDto>>>("assets");

            if (entities?.Data == null || entities.Data.Count == 0)
            {
                return;
            }

            var cryptoCurrencies = _mapper.Map<List<CryptoCurrency>>(entities.Data);
            var addedAndUpdatedCount = 0;

            foreach (var newCryptoCurrency in cryptoCurrencies)
            {
                if (!await _unitOfWork.CryptoCurrencies.IsCoinCapIdExistAsync(newCryptoCurrency.CoinCapId!))
                {
                    if (await _unitOfWork.CryptoCurrencies.IsNameAndSymbolExistAsync(newCryptoCurrency.Name, newCryptoCurrency.Symbol))
                    {
                        var cryptoCurrency = await _unitOfWork.CryptoCurrencies.GetByNameAndSymbolAsync(newCryptoCurrency.Name, newCryptoCurrency.Symbol);
                        cryptoCurrency!.CoinCapId = newCryptoCurrency.CoinCapId;

                        _unitOfWork.CryptoCurrencies.Update(cryptoCurrency);
                    }
                    else
                    {
                        newCryptoCurrency.Id = Guid.NewGuid();

                        await _unitOfWork.CryptoCurrencies.AddAsync(newCryptoCurrency);
                    }

                    addedAndUpdatedCount++;
                }
            }

            if (addedAndUpdatedCount > 0)
            {
                await _unitOfWork.SaveAsync();

                Log.Information($"Successfully retrieved cryptocurrency data from CoinCap Api");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while getting cryptocurrency data from CoinCap Api");
            throw;
        }
    }

    public async Task GetMarketDataAsync()
    {
        try
        {
            var entities = await _externalApiHttpClient.GetDataAsync<CoinCapResponseDto<List<CoinCapAssetResponseDto>>>("assets");

            if (entities?.Data == null || entities.Data.Count == 0)
            {
                return;
            }

            var addedCount = 0;

            foreach (var entity in entities.Data)
            {
                if (await _unitOfWork.CryptoCurrencies.IsCoinCapIdExistAsync(entity.Id))
                {
                    var cryptoCurrency = (await _unitOfWork.CryptoCurrencies.GetByCoinCapIdAsync(entity.Id))!;
                    var newMarketData = _mapper.Map<MarketData>(entity);

                    newMarketData.CryptoCurrencyId = cryptoCurrency.Id;

                    if (!await _unitOfWork.MarketData.IsUpdatedTodayAsync(newMarketData))
                    {
                        newMarketData.Id = Guid.NewGuid();
                        newMarketData.CryptoCurrencyId = cryptoCurrency.Id;

                        await _unitOfWork.MarketData.AddAsync(newMarketData);
                        addedCount++;
                    }
                }
            }

            if (addedCount > 0)
            {
                await _unitOfWork.SaveAsync();

                Log.Information($"Market data successfully retrieved and changes applied to the database from CoinCap Api");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while getting market data from CoinCap Api");
            throw;
        }
    }

    public async Task GetPriceHistoryByIdAsync(CoinCapHistoryDataRequestDto dto)
    {
        try
        {
            var cryptoCurrency = await _unitOfWork.CryptoCurrencies.GetByCoinCapIdAsync(dto.CoinCapId);
            
            if (cryptoCurrency == null)
            {
                return;
            }

            var entities = await _externalApiHttpClient.GetDataAsync<CoinCapResponseDto<List<CoinCapHistoryDataResponseDto>>>(
                $"/v3/assets/{dto.CoinCapId}/history?interval=d1");

            if (entities?.Data == null || entities.Data.Count == 0)
            {
                return;
            }

            var priceHistoryData = _mapper.Map<List<PriceHistoryData>>(entities.Data);
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

                Log.Information($"Successfully retrieved price history data from CoinCap Api");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while getting price history data from CoinCap Api");
            throw;
        }
    }
}