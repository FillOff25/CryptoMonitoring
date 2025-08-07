using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoMonitoring.Common.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexOfNameInCryptoCurrencyModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_crypto_currencies_name",
                table: "crypto_currencies",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_crypto_currencies_name",
                table: "crypto_currencies");
        }
    }
}
