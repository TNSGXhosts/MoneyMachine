using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestEnv.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePriceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Epic",
                table: "Prices",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PriceOrder",
                table: "Prices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Resolution",
                table: "Prices",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "UN_PriceOrder_Epic_Resolution",
                table: "Prices",
                columns: new[] { "PriceOrder", "Epic", "Resolution" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UN_PriceOrder_Epic_Resolution",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "Epic",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "PriceOrder",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "Resolution",
                table: "Prices");
        }
    }
}
