using AutoMapper;
using CryptoMonitoring.Common.DTOs.CoinGeckoApi;
using CryptoMonitoring.DataProcessor.Business.Interfaces.Processing;
using CryptoMonitoring.Common.Persistence.Interfaces;
using CryptoMonitoring.Models.Entities;
using Serilog;

namespace CryptoMonitoring.DataProcessor.Business.Services.Processing;

public class CoinGeckoProcessingService : ICoinGeckoProcessingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CoinGeckoProcessingService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetCoinGeckoDataResponseDto> ProcessCryptoCurrencyAsync(List<CoinGeckoCoinResponseDto> dto)
    {
        var newCryptoCurrencies = _mapper.Map<List<CryptoCurrency>>(dto);
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

    public async Task<GetCoinGeckoMarketDataByIdResponseDto> ProcessMarketDataByIdAsync(CoinGeckoMarketDataByIdRabbitMQResponseDto dto)
    {
        var cryptoCurrency = (await _unitOfWork.CryptoCurrencies.GetByCoinGeckoIdAsync(dto.CoinGeckoId))!;

        var newMarketData = _mapper.Map<List<MarketData>>(dto.Dto);

        newMarketData[0].CryptoCurrencyId = cryptoCurrency.Id;
        
        if (!await _unitOfWork.MarketData.IsUpdatedTodayAsync(newMarketData[0].CryptoCurrencyId, newMarketData[0].Timestamp))
        {
            newMarketData[0].Id = Guid.NewGuid();
            newMarketData[0].CryptoCurrencyId = cryptoCurrency.Id;

            await _unitOfWork.MarketData.AddAsync(newMarketData[0]);
            await _unitOfWork.SaveAsync();

            Log.Information($"Market data successfully retrieved and changes applied to the database from CoinGecko Api");
        }
        else
        {
            Log.Information($"This CoinGecko market data already exist in database");
        }

            return new GetCoinGeckoMarketDataByIdResponseDto(cryptoCurrency.Name, cryptoCurrency.Symbol, newMarketData[0].Timestamp);
    }

    public async Task<GetCoinGeckoPriceHistoryDataByIdResponseDto> ProcessPriceHistoryDataByIdAsync(CoinGeckoPriceHistoryDataByIdRabbitMQResponseDto dto)
    {
        var cryptoCurrency = (await _unitOfWork.CryptoCurrencies.GetByCoinGeckoIdAsync(dto.CoinGeckoId))!;

        var priceHistoryData = _mapper.Map<List<PriceHistoryData>>(dto.Dto.Prices);
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
        else
        {
            Log.Information($"Successfully retrieved price history data from CoinGecko Api, but no new data were added as all already existed");
        }

        return new GetCoinGeckoPriceHistoryDataByIdResponseDto(cryptoCurrency.Name, cryptoCurrency.Symbol, addedCount);
    }
}
