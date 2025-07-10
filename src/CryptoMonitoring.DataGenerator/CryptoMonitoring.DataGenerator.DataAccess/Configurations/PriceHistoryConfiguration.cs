using CryptoMonitoring.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoMonitoring.DataGenerator.DataAccess.Configurations;

public class PriceHistoryConfiguration : IEntityTypeConfiguration<PriceHistory>
{
    public void Configure(EntityTypeBuilder<PriceHistory> builder)
    {
        builder.ToTable("price_histories");

        builder.HasKey(ph => new { ph.CryptoCurrencyId, ph.Timestamp });

        builder.Property(ph => ph.CryptoCurrencyId)
            .HasColumnName("crypto_currency_id")
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder.Property(ph => ph.Timestamp)
            .HasColumnName("timestamp")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(ph => ph.PriceUsd)
            .HasColumnName("price_usd")
            .HasColumnType("decimal");
    }
}
