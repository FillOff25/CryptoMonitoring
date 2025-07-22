using CryptoMonitoring.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoMonitoring.DataGenerator.Persistence.Configurations;

public class PriceHistoryDataConfiguration : IEntityTypeConfiguration<PriceHistoryData>
{
    public void Configure(EntityTypeBuilder<PriceHistoryData> builder)
    {
        builder.ToTable("price_history_datas");

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

        builder.Property(phd => phd.CirculatingSupply)
            .HasColumnName("circulating_supply")
            .HasColumnType("decimal");

        builder.HasOne(phd => phd.CryptoCurrency)
            .WithMany()
            .HasForeignKey(phd => phd.CryptoCurrencyId);

        builder.HasIndex(phd => new { phd.CryptoCurrencyId, phd.Timestamp })
            .IsUnique();
    }
}
