using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trading.Application.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitCommit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PriceBatches",
                columns: table => new
                {
                    PriceBatchId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ticker = table.Column<string>(type: "TEXT", nullable: false),
                    TimeFrame = table.Column<string>(type: "TEXT", nullable: false),
                    Period = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_PriceBatches", x => x.PriceBatchId));

            migrationBuilder.CreateTable(
                name: "TradingVolumes",
                columns: table => new
                {
                    VolumesId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Bid = table.Column<decimal>(type: "TEXT", nullable: false),
                    Ask = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_TradingVolumes", x => x.VolumesId));

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    PriceId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SnapshotTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SnapshotTimeUTC = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OpenPriceVolumesId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClosePriceVolumesId = table.Column<int>(type: "INTEGER", nullable: false),
                    HighPriceVolumesId = table.Column<int>(type: "INTEGER", nullable: false),
                    LowPriceVolumesId = table.Column<int>(type: "INTEGER", nullable: false),
                    LastTradedVolume = table.Column<decimal>(type: "TEXT", nullable: false),
                    PriceBatchId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.PriceId);
                    table.ForeignKey(
                        name: "FK_Prices_PriceBatches_PriceBatchId",
                        column: x => x.PriceBatchId,
                        principalTable: "PriceBatches",
                        principalColumn: "PriceBatchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prices_TradingVolumes_ClosePriceVolumesId",
                        column: x => x.ClosePriceVolumesId,
                        principalTable: "TradingVolumes",
                        principalColumn: "VolumesId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prices_TradingVolumes_HighPriceVolumesId",
                        column: x => x.HighPriceVolumesId,
                        principalTable: "TradingVolumes",
                        principalColumn: "VolumesId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prices_TradingVolumes_LowPriceVolumesId",
                        column: x => x.LowPriceVolumesId,
                        principalTable: "TradingVolumes",
                        principalColumn: "VolumesId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prices_TradingVolumes_OpenPriceVolumesId",
                        column: x => x.OpenPriceVolumesId,
                        principalTable: "TradingVolumes",
                        principalColumn: "VolumesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceBatches_Ticker_TimeFrame_Period",
                table: "PriceBatches",
                columns: new[] { "Ticker", "TimeFrame", "Period" });

            migrationBuilder.CreateIndex(
                name: "IX_Prices_ClosePriceVolumesId",
                table: "Prices",
                column: "ClosePriceVolumesId");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_HighPriceVolumesId",
                table: "Prices",
                column: "HighPriceVolumesId");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_LowPriceVolumesId",
                table: "Prices",
                column: "LowPriceVolumesId");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_OpenPriceVolumesId",
                table: "Prices",
                column: "OpenPriceVolumesId");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_PriceBatchId",
                table: "Prices",
                column: "PriceBatchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "PriceBatches");

            migrationBuilder.DropTable(
                name: "TradingVolumes");
        }
    }
}
