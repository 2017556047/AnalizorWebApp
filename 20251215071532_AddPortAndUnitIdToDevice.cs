using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnalizorWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPortAndUnitIdToDevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Port",
                table: "Devices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte>(
                name: "UnitId",
                table: "Devices",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Port",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Devices");
        }
    }
}
