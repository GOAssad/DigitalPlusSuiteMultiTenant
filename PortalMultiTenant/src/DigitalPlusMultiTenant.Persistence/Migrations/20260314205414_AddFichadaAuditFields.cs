using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalPlusMultiTenant.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFichadaAuditFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Fichada",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificadoAt",
                table: "Fichada",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModificadoPor",
                table: "Fichada",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Fichada");

            migrationBuilder.DropColumn(
                name: "ModificadoAt",
                table: "Fichada");

            migrationBuilder.DropColumn(
                name: "ModificadoPor",
                table: "Fichada");
        }
    }
}
