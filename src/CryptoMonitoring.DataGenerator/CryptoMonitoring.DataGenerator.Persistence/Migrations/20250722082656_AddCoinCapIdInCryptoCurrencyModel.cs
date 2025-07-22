using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoMonitoring.DataGenerator.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCoinCapIdInCryptoCurrencyModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "price_usd",
                table: "price_history_datas",
                type: "numeric(100,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(100,0)");

            migrationBuilder.AddColumn<string>(
                name: "coin_cap_id",
                table: "crypto_currencies",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_crypto_currencies_symbol",
                table: "crypto_currencies",
                column: "symbol",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_crypto_currencies_symbol",
                table: "crypto_currencies");

            migrationBuilder.DropColumn(
                name: "coin_cap_id",
                table: "crypto_currencies");

            migrationBuilder.AlterColumn<decimal>(
                name: "price_usd",
                table: "price_history_datas",
                type: "numeric(100,0)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(100,0)",
                oldNullable: true);
        }
    }
}
