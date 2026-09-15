using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRowVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Subastas");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Billeteras");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Subastas",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Billeteras",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Subastas");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Billeteras");

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Subastas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Billeteras",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Billeteras",
                keyColumn: "Id",
                keyValue: 1,
                column: "Version",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Billeteras",
                keyColumn: "Id",
                keyValue: 2,
                column: "Version",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Billeteras",
                keyColumn: "Id",
                keyValue: 3,
                column: "Version",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Billeteras",
                keyColumn: "Id",
                keyValue: 4,
                column: "Version",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Version",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 2,
                column: "Version",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 3,
                column: "Version",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 4,
                column: "Version",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 5,
                column: "Version",
                value: 1);
        }
    }
}
