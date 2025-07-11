using CryptoMonitoring.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoMonitoring.DataGenerator.DataAccess.Configurations;

public class CryptoCurrencyConfiguration : IEntityTypeConfiguration<CryptoCurrency>
{
    public void Configure(EntityTypeBuilder<CryptoCurrency> builder)
    {
        builder.ToTable("crypto_currencies");

        builder.HasKey(cc => cc.Id);

        builder.Property(cc => cc.Id)
            .HasColumnName("id")
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(cc => cc.Name)
            .HasColumnName("name")
            .HasColumnType("varchar(100)");

        builder.Property(cc => cc.Symbol)
            .HasColumnName("symbol")
            .HasColumnType("varchar(100)");
    }
}