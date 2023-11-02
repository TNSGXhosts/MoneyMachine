using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestEnv.Migrations
{
    /// <inheritdoc />
    public partial class RemovePriceOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UN_PriceOrder_Epic_Resolution",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "PriceOrder",
                table: "Prices");

            migrationBuilder.CreateIndex(
                name: "UN_Epic_Resolution",
                table: "Prices",
                columns: new[] { "Epic", "Resolution" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UN_Epic_Resolution",
                table: "Prices");

            migrationBuilder.AddColumn<int>(
                name: "PriceOrder",
                table: "Prices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "UN_PriceOrder_Epic_Resolution",
                table: "Prices",
                columns: new[] { "PriceOrder", "Epic", "Resolution" },
                unique: true);
        }
    }
}
