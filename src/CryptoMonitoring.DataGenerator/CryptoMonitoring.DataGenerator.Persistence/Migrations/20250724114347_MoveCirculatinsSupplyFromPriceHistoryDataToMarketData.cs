using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoMonitoring.DataGenerator.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MoveCirculatinsSupplyFromPriceHistoryDataToMarketData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "circulating_supply",
                table: "price_history_datas");

            migrationBuilder.AddColumn<decimal>(
                name: "circulating_supply",
                table: "market_datas",
                type: "decimal",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "circulating_supply",
                table: "market_datas");

            migrationBuilder.AddColumn<decimal>(
                name: "circulating_supply",
                table: "price_history_datas",
                type: "decimal",
                nullable: true);
        }
    }
}
