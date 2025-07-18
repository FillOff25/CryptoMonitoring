using AutoMapper;
using CryptoMonitoring.DataGenerator.Business.DTOs.CoinCapApi;
using CryptoMonitoring.DataGenerator.Business.Interfaces.ExternalApis;
using CryptoMonitoring.DataGenerator.Persistence.Interfaces;
using CryptoMonitoring.Models.Entities;
using Microsoft.Extensions.Configuration;

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
        var entities = await _externalApiHttpClient.GetDataAsync<CoinCapAssetsResponse>("assets");

        if (entities !=  null)
        {
            var cryptoCurrencies = _mapper.Map<List<CryptoCurrency>>(entities.Data);
            var addedCount = 0;

            foreach (var cryptoCurrency in cryptoCurrencies)
            {
                if (!await _unitOfWork.CryptoCurrencies.IsNameExistAsync(cryptoCurrency.Name))
                {
                    cryptoCurrency.Id = Guid.NewGuid();

                    await _unitOfWork.CryptoCurrencies.AddAsync(cryptoCurrency);
                    addedCount++;
                }
            }

            if (addedCount > 0)
            {
                await _unitOfWork.SaveAsync();
            }
        }
    }

    public async Task GetMarketDataAsync()
    {
        var entities = await _externalApiHttpClient.GetDataAsync<CoinCapAssetsResponse>("assets");

        if (entities != null)
        {
            foreach (var entity in entities.Data)
            {
                if (await _unitOfWork.CryptoCurrencies.IsNameExistAsync(entity.Name))
                {
                    var cryptoCurrency = (await _unitOfWork.CryptoCurrencies.GetByNameAsync(entity.Name))!;
                    var newMarketData = _mapper.Map<MarketData>(entity);

                    newMarketData.CryptoCurrencyId = cryptoCurrency.Id;

                    if (await _unitOfWork.MarketDatas.IsUpdatedTodayAsync(newMarketData))
                    {
                        var marketData = await _unitOfWork.MarketDatas.GetByCryptoCurrencyIdAndTimestamp(cryptoCurrency.Id, newMarketData.Timestamp);

                        newMarketData.Id = marketData!.Id;
                        newMarketData.CryptoCurrencyId = marketData!.CryptoCurrencyId;

                        _unitOfWork.MarketDatas.Update(newMarketData);
                    }
                    else
                    {
                        newMarketData.Id = Guid.NewGuid();
                        newMarketData.CryptoCurrencyId = cryptoCurrency.Id;

                        await _unitOfWork.MarketDatas.AddAsync(newMarketData);
                    }
                }
            }
            
            await _unitOfWork.SaveAsync();
        }
    }
}