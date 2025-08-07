using CryptoMonitoring.Common.DTOs.CoinCapApi;

namespace CryptoMonitoring.DataProcessor.Business.Interfaces.Processing;

public interface ICoinCapProcessingService
{
    Task<GetCoinCapDataResponseDto> ProcessMarketDataAsync(CoinCapResponseDto<List<CoinCapAssetResponseDto>> dto);
    Task<GetCoinCapDataResponseDto> ProcessPriceHistoryByIdAsync(CoinCapPriceHistoryDataRabbitMQResponseDto dto);
    Task<GetCoinCapDataResponseDto> ProcessCryptoCurrencyAsync(CoinCapResponseDto<List<CoinCapAssetResponseDto>> dto);
}