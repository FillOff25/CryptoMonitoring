using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoMonitoring.Common.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ReworkUniqueIndexInCryptoCurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_crypto_currencies_name",
                table: "crypto_currencies");

            migrationBuilder.CreateIndex(
                name: "IX_crypto_currencies_name_symbol",
                table: "crypto_currencies",
                columns: new[] { "name", "symbol" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_crypto_currencies_name_symbol",
                table: "crypto_currencies");

            migrationBuilder.CreateIndex(
                name: "IX_crypto_currencies_name",
                table: "crypto_currencies",
                column: "name",
                unique: true);
        }
    }
}
