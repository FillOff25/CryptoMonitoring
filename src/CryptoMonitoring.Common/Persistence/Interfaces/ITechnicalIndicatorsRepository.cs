using CryptoMonitoring.Models.Entities;
using CryptoMonitoring.Models.Enums;

namespace CryptoMonitoring.Common.Persistence.Interfaces;

public interface ITechnicalIndicatorsRepository : IGenericRepository<TechnicalIndicator, Guid>
{
    Task<bool> IsExist(Guid cryptoCurrencyId, IndicatorTypeEnum type, DateTime timestamp);
}