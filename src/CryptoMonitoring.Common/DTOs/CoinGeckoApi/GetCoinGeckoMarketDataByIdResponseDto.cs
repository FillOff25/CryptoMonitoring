namespace CryptoMonitoring.Common.DTOs.CoinGeckoApi;

public record GetCoinGeckoMarketDataByIdResponseDto(
    string Name,
    string Symbol,
    DateTime Timestamp);