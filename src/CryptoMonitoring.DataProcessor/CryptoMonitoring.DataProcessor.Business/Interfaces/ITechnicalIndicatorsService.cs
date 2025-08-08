using CryptoMonitoring.Common.DTOs;

namespace CryptoMonitoring.DataProcessor.Business.Interfaces
{
    public interface ITechnicalIndicatorsService
    {
        Task CalculateTechnicalIndicatorAsync(CalculateTechnicalIndicatorRequestDto dto);
    }
}