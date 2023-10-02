using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregarFechaCreacionActualiacionStringNombreEspecialidad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Especialidads",
                table: "Especialidads");

            migrationBuilder.RenameTable(
                name: "Especialidads",
                newName: "Especialidad");

            migrationBuilder.AlterColumn<string>(
                name: "NombreEspecialidad",
                table: "Especialidad",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 60);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaActualizacion",
                table: "Especialidad",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Especialidad",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Especialidad",
                table: "Especialidad",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Especialidad",
                table: "Especialidad");

            migrationBuilder.DropColumn(
                name: "FechaActualizacion",
                table: "Especialidad");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Especialidad");

            migrationBuilder.RenameTable(
                name: "Especialidad",
                newName: "Especialidads");

            migrationBuilder.AlterColumn<int>(
                name: "NombreEspecialidad",
                table: "Especialidads",
                type: "int",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Especialidads",
                table: "Especialidads",
                column: "Id");
        }
    }
}
