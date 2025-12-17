using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnalizorWebApp.Migrations
{
    /// <inheritdoc />
    public partial class EnergySnapshotsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EnergySnapshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceId = table.Column<int>(type: "int", nullable: false),
                    EnergyT1 = table.Column<double>(type: "float", nullable: false),
                    SnapshotTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnergySnapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnergySnapshots_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnergySnapshots_DeviceId",
                table: "EnergySnapshots",
                column: "DeviceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnergySnapshots");
        }
    }
}
