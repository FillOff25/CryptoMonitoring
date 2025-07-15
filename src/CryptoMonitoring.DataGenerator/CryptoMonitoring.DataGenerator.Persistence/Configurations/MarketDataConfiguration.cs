using CryptoMonitoring.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoMonitoring.DataGenerator.Persistence.Configurations;

public class MarketDataConfiguration : IEntityTypeConfiguration<MarketData>
{
    public void Configure(EntityTypeBuilder<MarketData> builder)
    {
        builder.ToTable("market_datas");

        builder.HasKey(md => md.Id);

        builder.Property(md => md.Id)
            .HasColumnName("id")
            .HasColumnType("uuid");

        builder.Property(md => md.Timestamp)
            .HasColumnName("timestamp")
            .HasColumnType("timestamp with time zone");

        builder.Property(md => md.PriceUsd)
            .HasColumnName("price_usd")
            .HasColumnType("decimal(100)");

        builder.Property(md => md.Volume24hUsd)
            .HasColumnName("volume_24h_usd")
            .HasColumnType("decimal");

        builder.Property(md => md.MarketCapUsd)
            .HasColumnName("market_cap_usd")
            .HasColumnType("decimal");

        builder.Property(md => md.Vwap24hUsd)
            .HasColumnName("vwap_24h_usd")
            .HasColumnType("decimal");

        builder.Property(md => md.CirculatingSupply)
            .HasColumnName("circulating_supply")
            .HasColumnType("decimal");

        builder.Property(md => md.Change24hPercent)
            .HasColumnName("сhange_24h_зercent")
            .HasColumnType("decimal");

        builder.HasOne(md => md.CryptoCurrency)
            .WithMany()
            .HasForeignKey(md => md.CryptoCurrencyId);

        builder.HasIndex(md => new { md.CryptoCurrencyId, md.Timestamp })
            .IsUnique();
    }
}
