using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoMonitoring.Common.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddHistoryPriceData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "price_usd",
                table: "market_datas");

            migrationBuilder.AlterColumn<decimal>(
                name: "volume_24h_usd",
                table: "market_datas",
                type: "decimal",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal");

            migrationBuilder.AlterColumn<decimal>(
                name: "market_cap_usd",
                table: "market_datas",
                type: "decimal",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal");

            migrationBuilder.CreateTable(
                name: "price_history_datas",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    price_usd = table.Column<decimal>(type: "numeric(100,0)", nullable: false),
                    CryptoCurrencyId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_price_history_datas", x => x.id);
                    table.ForeignKey(
                        name: "FK_price_history_datas_crypto_currencies_CryptoCurrencyId",
                        column: x => x.CryptoCurrencyId,
                        principalTable: "crypto_currencies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_price_history_datas_CryptoCurrencyId_timestamp",
                table: "price_history_datas",
                columns: new[] { "CryptoCurrencyId", "timestamp" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "price_history_datas");

            migrationBuilder.AlterColumn<decimal>(
                name: "volume_24h_usd",
                table: "market_datas",
                type: "decimal",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "market_cap_usd",
                table: "market_datas",
                type: "decimal",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "price_usd",
                table: "market_datas",
                type: "numeric(100,0)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
