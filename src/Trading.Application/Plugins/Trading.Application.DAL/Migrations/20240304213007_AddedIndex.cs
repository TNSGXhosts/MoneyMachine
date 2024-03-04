using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trading.Application.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Prices_Ticker_TimeFrame_SnapshotTime",
                table: "Prices",
                columns: new[] { "Ticker", "TimeFrame", "SnapshotTime" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Prices_Ticker_TimeFrame_SnapshotTime",
                table: "Prices");
        }
    }
}
