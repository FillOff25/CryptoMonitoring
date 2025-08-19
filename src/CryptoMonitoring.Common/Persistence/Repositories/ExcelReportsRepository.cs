using CryptoMonitoring.Common.Persistence.Databases;
using CryptoMonitoring.Common.Persistence.Interfaces;
using CryptoMonitoring.Models.Entities;
using CryptoMonitoring.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace CryptoMonitoring.Common.Persistence.Repositories;

public class ExcelReportsRepository : GenericRepository<ExcelReport, Guid>, IExcelReportsRepository
{
    public ExcelReportsRepository(CryptoMonitoringDataDbContext dbContext)
        : base(dbContext)
    { }

    public async Task<ExcelReport?> GetReportAsync(Guid cryptoCurrencyId, ExcelReportTypeEnum reportType, DateTime createdAt)
    {
        return await DbSet.AsNoTracking()
            .FirstOrDefaultAsync(er =>
                er.CryptoCurrencyId == cryptoCurrencyId &&
                er.ReportType == reportType &&
                er.CreatedAt.Date == createdAt.Date);
    }
}
