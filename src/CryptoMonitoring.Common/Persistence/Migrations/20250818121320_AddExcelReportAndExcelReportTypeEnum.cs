using System;
using CryptoMonitoring.Models.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoMonitoring.Common.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddExcelReportAndExcelReportTypeEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_market_datas_crypto_currencies_CryptoCurrencyId",
                table: "market_datas");

            migrationBuilder.DropForeignKey(
                name: "FK_price_history_datas_crypto_currencies_CryptoCurrencyId",
                table: "price_history_datas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_price_history_datas",
                table: "price_history_datas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_market_datas",
                table: "market_datas");

            migrationBuilder.RenameTable(
                name: "price_history_datas",
                newName: "price_history_data");

            migrationBuilder.RenameTable(
                name: "market_datas",
                newName: "market_data");

            migrationBuilder.RenameIndex(
                name: "IX_price_history_datas_CryptoCurrencyId_timestamp",
                table: "price_history_data",
                newName: "IX_price_history_data_CryptoCurrencyId_timestamp");

            migrationBuilder.RenameColumn(
                name: "сhange_24h_зercent",
                table: "market_data",
                newName: "сhange_24h_percent");

            migrationBuilder.RenameIndex(
                name: "IX_market_datas_CryptoCurrencyId_timestamp",
                table: "market_data",
                newName: "IX_market_data_CryptoCurrencyId_timestamp");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:excel_report_type_enum", "comparative_analysis,daily_report,portfolio_analysis,technical_analisis,volatility_report")
                .Annotation("Npgsql:Enum:indicator_type_enum", "ema,rsi,sma")
                .OldAnnotation("Npgsql:Enum:indicator_type_enum", "ema,rsi,sma");

            migrationBuilder.AddPrimaryKey(
                name: "PK_price_history_data",
                table: "price_history_data",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_market_data",
                table: "market_data",
                column: "id");

            migrationBuilder.CreateTable(
                name: "excel_reports",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_path = table.Column<string>(type: "varchar(100)", nullable: false),
                    file_url = table.Column<string>(type: "varchar(100)", nullable: false),
                    file_type = table.Column<string>(type: "varchar(100)", nullable: false),
                    original_file_name = table.Column<string>(type: "varchar(100)", nullable: false),
                    report_type = table.Column<ExcelReportTypeEnum>(type: "excel_report_type_enum", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CryptoCurrencyId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_excel_reports", x => x.id);
                    table.ForeignKey(
                        name: "FK_excel_reports_crypto_currencies_CryptoCurrencyId",
                        column: x => x.CryptoCurrencyId,
                        principalTable: "crypto_currencies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_excel_reports_CryptoCurrencyId",
                table: "excel_reports",
                column: "CryptoCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_excel_reports_report_type_CryptoCurrencyId_created_at",
                table: "excel_reports",
                columns: new[] { "report_type", "CryptoCurrencyId", "created_at" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_market_data_crypto_currencies_CryptoCurrencyId",
                table: "market_data",
                column: "CryptoCurrencyId",
                principalTable: "crypto_currencies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_price_history_data_crypto_currencies_CryptoCurrencyId",
                table: "price_history_data",
                column: "CryptoCurrencyId",
                principalTable: "crypto_currencies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_market_data_crypto_currencies_CryptoCurrencyId",
                table: "market_data");

            migrationBuilder.DropForeignKey(
                name: "FK_price_history_data_crypto_currencies_CryptoCurrencyId",
                table: "price_history_data");

            migrationBuilder.DropTable(
                name: "excel_reports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_price_history_data",
                table: "price_history_data");

            migrationBuilder.DropPrimaryKey(
                name: "PK_market_data",
                table: "market_data");

            migrationBuilder.RenameTable(
                name: "price_history_data",
                newName: "price_history_datas");

            migrationBuilder.RenameTable(
                name: "market_data",
                newName: "market_datas");

            migrationBuilder.RenameIndex(
                name: "IX_price_history_data_CryptoCurrencyId_timestamp",
                table: "price_history_datas",
                newName: "IX_price_history_datas_CryptoCurrencyId_timestamp");

            migrationBuilder.RenameColumn(
                name: "сhange_24h_percent",
                table: "market_datas",
                newName: "сhange_24h_зercent");

            migrationBuilder.RenameIndex(
                name: "IX_market_data_CryptoCurrencyId_timestamp",
                table: "market_datas",
                newName: "IX_market_datas_CryptoCurrencyId_timestamp");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:indicator_type_enum", "ema,rsi,sma")
                .OldAnnotation("Npgsql:Enum:excel_report_type_enum", "comparative_analysis,daily_report,portfolio_analysis,technical_analisis,volatility_report")
                .OldAnnotation("Npgsql:Enum:indicator_type_enum", "ema,rsi,sma");

            migrationBuilder.AddPrimaryKey(
                name: "PK_price_history_datas",
                table: "price_history_datas",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_market_datas",
                table: "market_datas",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_market_datas_crypto_currencies_CryptoCurrencyId",
                table: "market_datas",
                column: "CryptoCurrencyId",
                principalTable: "crypto_currencies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_price_history_datas_crypto_currencies_CryptoCurrencyId",
                table: "price_history_datas",
                column: "CryptoCurrencyId",
                principalTable: "crypto_currencies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
