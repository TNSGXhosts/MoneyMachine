using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trading.Application.DAL.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ticker",
                table: "Prices",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TimeFrame",
                table: "Prices",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ticker",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "TimeFrame",
                table: "Prices");
        }
    }
}
