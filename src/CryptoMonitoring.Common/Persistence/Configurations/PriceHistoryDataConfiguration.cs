using CryptoMonitoring.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoMonitoring.Common.Persistence.Configurations;

public class PriceHistoryDataConfiguration : IEntityTypeConfiguration<PriceHistoryData>
{
    public void Configure(EntityTypeBuilder<PriceHistoryData> builder)
    {
        builder.ToTable("price_history_data");

        builder.HasKey(phd => phd.Id);

        builder.Property(phd => phd.Id)
            .HasColumnName("id")
            .HasColumnType("uuid");

        builder.Property(phd => phd.Timestamp)
            .HasColumnName("timestamp")
            .HasColumnType("timestamp with time zone");

        builder.Property(phd => phd.PriceUsd)
            .HasColumnName("price_usd")
            .HasColumnType("decimal(100)");

        builder.HasOne(phd => phd.CryptoCurrency)
            .WithMany()
            .HasForeignKey(phd => phd.CryptoCurrencyId);

        builder.HasIndex(phd => new { phd.CryptoCurrencyId, phd.Timestamp })
            .IsUnique();
    }
}
