using System;
using CryptoMonitoring.Models.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoMonitoring.DataGenerator.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:indicator_type_enum", "bollinger_bands,ema,macd,rsi,sma");

            migrationBuilder.CreateTable(
                name: "crypto_currencies",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(100)", nullable: false),
                    symbol = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_crypto_currencies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "market_datas",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    price_usd = table.Column<decimal>(type: "numeric(100,0)", nullable: false),
                    volume_24h_usd = table.Column<decimal>(type: "decimal", nullable: false),
                    market_cap_usd = table.Column<decimal>(type: "decimal", nullable: false),
                    vwap_24h_usd = table.Column<decimal>(type: "decimal", nullable: true),
                    circulating_supply = table.Column<decimal>(type: "decimal", nullable: true),
                    сhange_24h_зercent = table.Column<decimal>(type: "decimal", nullable: true),
                    CryptoCurrencyId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_market_datas", x => x.id);
                    table.ForeignKey(
                        name: "FK_market_datas_crypto_currencies_CryptoCurrencyId",
                        column: x => x.CryptoCurrencyId,
                        principalTable: "crypto_currencies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "technical_indicators",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    indicator_type = table.Column<IndicatorTypeEnum>(type: "indicator_type_enum", nullable: false),
                    value = table.Column<decimal>(type: "decimal", nullable: false),
                    CryptoCurrencyId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_technical_indicators", x => x.id);
                    table.ForeignKey(
                        name: "FK_technical_indicators_crypto_currencies_CryptoCurrencyId",
                        column: x => x.CryptoCurrencyId,
                        principalTable: "crypto_currencies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_market_datas_CryptoCurrencyId_timestamp",
                table: "market_datas",
                columns: new[] { "CryptoCurrencyId", "timestamp" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_technical_indicators_CryptoCurrencyId_indicator_type_timest~",
                table: "technical_indicators",
                columns: new[] { "CryptoCurrencyId", "indicator_type", "timestamp" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "market_datas");

            migrationBuilder.DropTable(
                name: "technical_indicators");

            migrationBuilder.DropTable(
                name: "crypto_currencies");
        }
    }
}
