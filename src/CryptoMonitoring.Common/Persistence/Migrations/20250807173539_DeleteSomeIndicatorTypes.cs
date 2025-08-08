using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoMonitoring.Common.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DeleteSomeIndicatorTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM \"technical_indicators\" WHERE \"indicator_type\" IN ('bollinger_bands', 'macd');");
            migrationBuilder.Sql("CREATE TYPE \"public\".\"new_indicator_type_enum\" AS ENUM ('ema', 'rsi', 'sma');");
            migrationBuilder.Sql("ALTER TABLE \"technical_indicators\" ALTER COLUMN \"indicator_type\" TYPE \"public\".\"new_indicator_type_enum\" USING \"indicator_type\"::text::\"public\".\"new_indicator_type_enum\";");
            migrationBuilder.Sql("DROP TYPE \"public\".\"indicator_type_enum\";");
            migrationBuilder.Sql("ALTER TYPE \"public\".\"new_indicator_type_enum\" RENAME TO \"indicator_type_enum\";");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM \"technical_indicators\" WHERE \"indicator_type\" IN ('ema', 'rsi', 'sma');");
            migrationBuilder.Sql("CREATE TYPE \"public\".\"new_indicator_type_enum\" AS ENUM ('bollinger_bands', 'ema', 'macd', 'rsi', 'sma');");
            migrationBuilder.Sql("ALTER TABLE \"technical_indicators\" ALTER COLUMN \"indicator_type\" TYPE \"public\".\"new_indicator_type_enum\" USING \"indicator_type\"::text::\"public\".\"new_indicator_type_enum\";");
            migrationBuilder.Sql("DROP TYPE \"public\".\"indicator_type_enum\";");
            migrationBuilder.Sql("ALTER TYPE \"public\".\"new_indicator_type_enum\" RENAME TO \"indicator_type_enum\";");
        }
    }
}
