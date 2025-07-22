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
        _externalApiHttpClient.SetBearerApiKey(configuration["COINCAP_API_KEY"]!);

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
            var addedCount = 0;

            foreach (var cryptoCurrency in cryptoCurrencies)
            {
                if (!await _unitOfWork.CryptoCurrencies.IsCoinCapIdExistAsync(cryptoCurrency.CoinCapId!) &&
                    !await _unitOfWork.CryptoCurrencies.IsNameExistAsync(cryptoCurrency.Name))
                {
                    cryptoCurrency.Id = Guid.NewGuid();

                    await _unitOfWork.CryptoCurrencies.AddAsync(cryptoCurrency);
                    addedCount++;
                }
            }

            if (addedCount > 0)
            {
                await _unitOfWork.SaveAsync();

                Log.Information($"Successfully retrieved cryptocurrency data. {addedCount} new cryptocurrencies were added to the database.");
            }
            else
            {
                Log.Information("Successfully retrieved cryptocurrency data, but no new cryptocurrencies were added as all already existed.");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while getting cryptocurrency data");
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

            foreach (var entity in entities.Data)
            {
                if (await _unitOfWork.CryptoCurrencies.IsCoinCapIdExistAsync(entity.Id))
                {
                    var cryptoCurrency = (await _unitOfWork.CryptoCurrencies.GetByCoinCapIdAsync(entity.Id))!;
                    var newMarketData = _mapper.Map<MarketData>(entity);

                    newMarketData.CryptoCurrencyId = cryptoCurrency.Id;

                    if (await _unitOfWork.MarketData.IsUpdatedTodayAsync(newMarketData))
                    {
                        var marketData = await _unitOfWork.MarketData.GetByCryptoCurrencyIdAndTimestamp(cryptoCurrency.Id, newMarketData.Timestamp);

                        newMarketData.Id = marketData!.Id;
                        newMarketData.CryptoCurrencyId = marketData!.CryptoCurrencyId;

                        _unitOfWork.MarketData.Update(newMarketData);
                    }
                    else
                    {
                        newMarketData.Id = Guid.NewGuid();
                        newMarketData.CryptoCurrencyId = cryptoCurrency.Id;

                        await _unitOfWork.MarketData.AddAsync(newMarketData);
                    }
                }
            }

            await _unitOfWork.SaveAsync();

            Log.Information("Market data successfully retrieved and changes applied to the database.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while getting market data");
            throw;
        }
    }

    public async Task GetPriceHistoryAsync(CoinCapHistoryDataRequestDto dto)
    {
        try
        {
            if (!await _unitOfWork.CryptoCurrencies.IsCoinCapIdExistAsync(dto.Slug))
            {
                return;
            }

            var entities = await _externalApiHttpClient.GetDataAsync<CoinCapResponseDto<List<CoinCapHistoryDataResponseDto>>>(
                $"/v3/assets/{dto.Slug}/history?interval={dto.Interval}");

            if (entities?.Data == null || entities.Data.Count == 0)
            {
                return;
            }

            var priceHistoryData = _mapper.Map<List<PriceHistoryData>>(entities.Data);
            var cryptoCurrency = await _unitOfWork.CryptoCurrencies.GetByCoinCapIdAsync(dto.Slug);
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

                Log.Information($"Successfully retrieved price history data (slug: {dto.Slug}). {addedCount} new cryptocurrencies were added to the database.");
            }
            else
            {
                Log.Information("Successfully retrieved cryptocurrency data, but no new price history data were added as all already existed.");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while getting price history data");
            throw;
        }
    }
}