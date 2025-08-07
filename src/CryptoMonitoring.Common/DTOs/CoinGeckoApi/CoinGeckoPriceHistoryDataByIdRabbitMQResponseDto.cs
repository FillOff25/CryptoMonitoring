namespace CryptoMonitoring.Common.DTOs.CoinGeckoApi;

public record CoinGeckoPriceHistoryDataByIdRabbitMQResponseDto(
    string CoinGeckoId,
    CoinGeckoHistoricalChartDataResponseDto Dto);