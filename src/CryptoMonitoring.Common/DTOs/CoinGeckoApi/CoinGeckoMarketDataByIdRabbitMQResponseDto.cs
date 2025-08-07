namespace CryptoMonitoring.Common.DTOs.CoinGeckoApi;

public record CoinGeckoMarketDataByIdRabbitMQResponseDto(
    string CoinGeckoId,
    List<CoinGeckoCoinWithMarketDataResponseDto> Dto);