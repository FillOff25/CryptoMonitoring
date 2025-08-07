using AutoMapper;
using CryptoMonitoring.Common.DTOs.CoinCapApi;
using CryptoMonitoring.DataProcessor.Business.Interfaces.Processing;
using CryptoMonitoring.Common.Persistence.Interfaces;
using CryptoMonitoring.Models.Entities;
using Serilog;

namespace CryptoMonitoring.DataProcessor.Business.Services.Processing;

public class CoinCapProcessingService : ICoinCapProcessingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CoinCapProcessingService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetCoinCapDataResponseDto> ProcessCryptoCurrencyAsync(CoinCapResponseDto<List<CoinCapAssetResponseDto>> dto)
    {
        var cryptoCurrencies = _mapper.Map<List<CryptoCurrency>>(dto.Data);
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

        return new GetCoinCapDataResponseDto(addedAndUpdatedCount);
    }

    public async Task<GetCoinCapDataResponseDto> ProcessMarketDataAsync(CoinCapResponseDto<List<CoinCapAssetResponseDto>> dto)
    {
        var addedCount = 0;

        foreach (var entity in dto.Data!)
        {
            if (await _unitOfWork.CryptoCurrencies.IsCoinCapIdExistAsync(entity.Id))
            {
                var cryptoCurrency = (await _unitOfWork.CryptoCurrencies.GetByCoinCapIdAsync(entity.Id))!;
                var newMarketData = _mapper.Map<MarketData>(entity);

                newMarketData.CryptoCurrencyId = cryptoCurrency.Id;

                if (!await _unitOfWork.MarketData.IsUpdatedTodayAsync(newMarketData.CryptoCurrencyId, newMarketData.Timestamp))
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

        return new GetCoinCapDataResponseDto(addedCount);
    }

    public async Task<GetCoinCapDataResponseDto> ProcessPriceHistoryByIdAsync(CoinCapPriceHistoryDataRabbitMQResponseDto dto)
    {
        var cryptoCurrency = await _unitOfWork.CryptoCurrencies.GetByCoinCapIdAsync(dto.CoinCapId)!;

        var priceHistoryData = _mapper.Map<List<PriceHistoryData>>(dto.Dto.Data);
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

        return new GetCoinCapDataResponseDto(addedCount);
    }
}
