using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trading.Application.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TradingVolumes",
                columns: table => new
                {
                    VolumesId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Bid = table.Column<double>(type: "REAL", nullable: false),
                    Ask = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_TradingVolumes", x => x.VolumesId));

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    PriceId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SnapshotTime = table.Column<string>(type: "TEXT", nullable: false),
                    SnapshotTimeUTC = table.Column<string>(type: "TEXT", nullable: false),
                    OpenPriceVolumesId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClosePriceVolumesId = table.Column<int>(type: "INTEGER", nullable: false),
                    HighPriceVolumesId = table.Column<int>(type: "INTEGER", nullable: false),
                    LowPriceVolumesId = table.Column<int>(type: "INTEGER", nullable: false),
                    LastTradedVolume = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.PriceId);
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "TradingVolumes");
        }
    }
}
