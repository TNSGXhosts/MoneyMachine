using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestEnv.Migrations
{
    /// <inheritdoc />
    public partial class AddedUniqueForSnapshotTime2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UN_Epic_Resolution",
                table: "Prices");

            migrationBuilder.CreateIndex(
                name: "UN_Epic_Resolution_SnapshotTime",
                table: "Prices",
                columns: new[] { "Epic", "Resolution", "SnapshotTime" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UN_Epic_Resolution_SnapshotTime",
                table: "Prices");

            migrationBuilder.CreateIndex(
                name: "UN_Epic_Resolution",
                table: "Prices",
                columns: new[] { "Epic", "Resolution" },
                unique: true);
        }
    }
}
