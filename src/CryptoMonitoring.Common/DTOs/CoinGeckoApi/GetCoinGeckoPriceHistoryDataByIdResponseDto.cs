namespace CryptoMonitoring.Common.DTOs.CoinGeckoApi;

public record GetCoinGeckoPriceHistoryDataByIdResponseDto(
    string Name,
    string Symbol,
    int AddedCount);