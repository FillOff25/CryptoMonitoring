using CryptoMonitoring.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoMonitoring.Common.Persistence.Configurations;

public class ExcelReportConfiguration : IEntityTypeConfiguration<ExcelReport>
{
    public void Configure(EntityTypeBuilder<ExcelReport> builder)
    {
        builder.ToTable("excel_reports");

        builder.HasKey(er => er.Id);

        builder.Property(er => er.Id)
            .HasColumnName("id")
            .HasColumnType("uuid");

        builder.Property(er => er.FilePath)
            .HasColumnName("file_path")
            .HasColumnType("varchar(100)");

        builder.Property(er => er.FileUrl)
            .HasColumnName("file_url")
            .HasColumnType("varchar(100)");

        builder.Property(er => er.FileType)
            .HasColumnName("file_type")
            .HasColumnType("varchar(100)");

        builder.Property(er => er.OriginalFileName)
            .HasColumnName("original_file_name")
            .HasColumnType("varchar(100)");

        builder.Property(er => er.ReportType)
            .HasColumnName("report_type")
            .HasColumnType("excel_report_type_enum");

        builder.Property(er => er.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone");

        builder.HasOne(er => er.CryptoCurrency)
            .WithMany()
            .HasForeignKey(er => er.CryptoCurrencyId);

        builder.HasIndex(er => new { er.ReportType, er.CryptoCurrencyId, er.CreatedAt })
            .IsUnique();
    }
}