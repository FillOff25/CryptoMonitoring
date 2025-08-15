using CryptoMonitoring.Common.DTOs.DataProcessor;

namespace CryptoMonitoring.DataProcessor.Business.Interfaces
{
    public interface ITechnicalIndicatorsService
    {
        Task CalculateTechnicalIndicatorAsync(CalculateTechnicalIndicatorRequestDto dto);
    }
}