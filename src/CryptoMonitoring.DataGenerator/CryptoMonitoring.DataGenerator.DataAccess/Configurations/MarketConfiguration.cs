using CryptoMonitoring.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoMonitoring.DataGenerator.DataAccess.Configurations;

public class MarketConfiguration : IEntityTypeConfiguration<Market>
{
    public void Configure(EntityTypeBuilder<Market> builder)
    {
        builder.ToTable("markets");

        builder.HasKey(m => new { m.MarketId, m.BaseSymbol, m.QuoteSymbol });

        builder.Property(m => m.MarketId)
            .HasColumnName("market_id")
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder.Property(m => m.BaseSymbol)
            .HasColumnName("base_symbol")
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder.Property(m => m.QuoteSymbol)
            .HasColumnName("quote_symbol")
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder.Property(m => m.PriceUsd)
            .HasColumnName("price_usd")
            .HasColumnType("decimal");

        builder.Property(m => m.VolumeUsd24Hr)
            .HasColumnName("volume_usd_24hr")
            .HasColumnType("decimal");

        builder.Property(m => m.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp with time zone");
    }
}
