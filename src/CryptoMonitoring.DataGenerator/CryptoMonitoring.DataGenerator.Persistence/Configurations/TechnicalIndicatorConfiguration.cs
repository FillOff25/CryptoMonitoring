using CryptoMonitoring.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoMonitoring.DataGenerator.Persistence.Configurations;

public class TechnicalIndicatorConfiguration : IEntityTypeConfiguration<TechnicalIndicator>
{
    public void Configure(EntityTypeBuilder<TechnicalIndicator> builder)
    {
        builder.ToTable("technical_indicators");

        builder.HasKey(ti => ti.Id);

        builder.Property(ti => ti.Id)
            .HasColumnName("id")
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(ti => ti.Timestamp)
            .HasColumnName("timestamp")
            .HasColumnType("timestamp with time zone");

        builder.Property(ti => ti.IndicatorType)
            .HasColumnName("indicator_type")
            .HasColumnType("indicator_type_enum");

        builder.Property(ti => ti.Value)
            .HasColumnName("value")
            .HasColumnType("decimal");

        builder.HasOne(ti => ti.CryptoCurrency)
            .WithMany()
            .HasForeignKey(ti => ti.CryptoCurrencyId);

        builder.HasIndex(ti => new { ti.CryptoCurrencyId, ti.IndicatorType, ti.Timestamp })
            .IsUnique();
    }
}
