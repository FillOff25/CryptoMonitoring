namespace CryptoMonitoring.Common.DTOs.CoinCapApi;

public record CoinCapPriceHistoryDataRabbitMQResponseDto(
    string CoinCapId,
    CoinCapResponseDto<List<CoinCapHistoryDataResponseDto>> Dto);