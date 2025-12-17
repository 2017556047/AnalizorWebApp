using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnalizorWebApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "Port",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Devices");

            migrationBuilder.RenameColumn(
                name: "V_L3",
                table: "Readings",
                newName: "V3");

            migrationBuilder.RenameColumn(
                name: "V_L2",
                table: "Readings",
                newName: "V2");

            migrationBuilder.RenameColumn(
                name: "V_L1",
                table: "Readings",
                newName: "V1");

            migrationBuilder.RenameColumn(
                name: "TotalActiveEnergy_T1",
                table: "Readings",
                newName: "P3");

            migrationBuilder.RenameColumn(
                name: "P_L3",
                table: "Readings",
                newName: "P2");

            migrationBuilder.RenameColumn(
                name: "P_L2",
                table: "Readings",
                newName: "P1");

            migrationBuilder.RenameColumn(
                name: "P_L1",
                table: "Readings",
                newName: "I3");

            migrationBuilder.RenameColumn(
                name: "I_L3",
                table: "Readings",
                newName: "I2");

            migrationBuilder.RenameColumn(
                name: "I_L2",
                table: "Readings",
                newName: "I1");

            migrationBuilder.RenameColumn(
                name: "I_L1",
                table: "Readings",
                newName: "EnergyT1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "V3",
                table: "Readings",
                newName: "V_L3");

            migrationBuilder.RenameColumn(
                name: "V2",
                table: "Readings",
                newName: "V_L2");

            migrationBuilder.RenameColumn(
                name: "V1",
                table: "Readings",
                newName: "V_L1");

            migrationBuilder.RenameColumn(
                name: "P3",
                table: "Readings",
                newName: "TotalActiveEnergy_T1");

            migrationBuilder.RenameColumn(
                name: "P2",
                table: "Readings",
                newName: "P_L3");

            migrationBuilder.RenameColumn(
                name: "P1",
                table: "Readings",
                newName: "P_L2");

            migrationBuilder.RenameColumn(
                name: "I3",
                table: "Readings",
                newName: "P_L1");

            migrationBuilder.RenameColumn(
                name: "I2",
                table: "Readings",
                newName: "I_L3");

            migrationBuilder.RenameColumn(
                name: "I1",
                table: "Readings",
                newName: "I_L2");

            migrationBuilder.RenameColumn(
                name: "EnergyT1",
                table: "Readings",
                newName: "I_L1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Devices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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
    }
}
