using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestEnv.Migrations
{
    /// <inheritdoc />
    public partial class CreateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    PriceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SnapshotTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SnapshotTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OpenPriceBid = table.Column<double>(type: "float", nullable: false),
                    OpenPriceAsk = table.Column<double>(type: "float", nullable: false),
                    ClosePriceBid = table.Column<double>(type: "float", nullable: false),
                    ClosePriceAsk = table.Column<double>(type: "float", nullable: false),
                    HighPriceBid = table.Column<double>(type: "float", nullable: false),
                    HighPriceAsk = table.Column<double>(type: "float", nullable: false),
                    LowPriceBid = table.Column<double>(type: "float", nullable: false),
                    LowPriceAsk = table.Column<double>(type: "float", nullable: false),
                    LastTradedVolume = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.PriceId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prices");
        }
    }
}
