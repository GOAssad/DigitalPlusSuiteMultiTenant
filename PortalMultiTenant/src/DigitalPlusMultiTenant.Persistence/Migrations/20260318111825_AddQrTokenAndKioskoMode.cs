using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalPlusMultiTenant.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddQrTokenAndKioskoMode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TerminalesMoviles_Legajo_LegajoId",
                table: "TerminalesMoviles");

            migrationBuilder.AlterColumn<string>(
                name: "PublicKey",
                table: "TerminalesMoviles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "LegajoId",
                table: "TerminalesMoviles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "ModoKiosko",
                table: "TerminalesMoviles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SucursalId",
                table: "TerminalesMoviles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QrToken",
                table: "Legajo",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TerminalesMoviles_SucursalId",
                table: "TerminalesMoviles",
                column: "SucursalId");

            // Backfill QrToken para legajos existentes
            migrationBuilder.Sql("UPDATE Legajo SET QrToken = LOWER(NEWID()) WHERE QrToken IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Legajo_QrToken",
                table: "Legajo",
                column: "QrToken",
                unique: true,
                filter: "[QrToken] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_TerminalesMoviles_Legajo_LegajoId",
                table: "TerminalesMoviles",
                column: "LegajoId",
                principalTable: "Legajo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TerminalesMoviles_Sucursal_SucursalId",
                table: "TerminalesMoviles",
                column: "SucursalId",
                principalTable: "Sucursal",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TerminalesMoviles_Legajo_LegajoId",
                table: "TerminalesMoviles");

            migrationBuilder.DropForeignKey(
                name: "FK_TerminalesMoviles_Sucursal_SucursalId",
                table: "TerminalesMoviles");

            migrationBuilder.DropIndex(
                name: "IX_TerminalesMoviles_SucursalId",
                table: "TerminalesMoviles");

            migrationBuilder.DropIndex(
                name: "IX_Legajo_QrToken",
                table: "Legajo");

            migrationBuilder.DropColumn(
                name: "ModoKiosko",
                table: "TerminalesMoviles");

            migrationBuilder.DropColumn(
                name: "SucursalId",
                table: "TerminalesMoviles");

            migrationBuilder.DropColumn(
                name: "QrToken",
                table: "Legajo");

            migrationBuilder.AlterColumn<string>(
                name: "PublicKey",
                table: "TerminalesMoviles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LegajoId",
                table: "TerminalesMoviles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TerminalesMoviles_Legajo_LegajoId",
                table: "TerminalesMoviles",
                column: "LegajoId",
                principalTable: "Legajo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
