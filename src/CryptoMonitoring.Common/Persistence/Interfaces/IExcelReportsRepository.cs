using CryptoMonitoring.Models.Entities;
using CryptoMonitoring.Models.Enums;

namespace CryptoMonitoring.Common.Persistence.Interfaces
{
    public interface IExcelReportsRepository : IGenericRepository<ExcelReport, Guid>
    {
        Task<ExcelReport?> GetReportAsync(Guid cryptoCurrencyId, ExcelReportTypeEnum reportType, DateTime createdAt);
    }
}