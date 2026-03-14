using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalPlusMultiTenant.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSucursalFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SucursalGeoconfigs_SucursalId",
                table: "SucursalGeoconfigs");

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "Sucursal",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Sucursal",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Localidad",
                table: "Sucursal",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Provincia",
                table: "Sucursal",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "Sucursal",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SucursalGeoconfigs_SucursalId",
                table: "SucursalGeoconfigs",
                column: "SucursalId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SucursalGeoconfigs_SucursalId",
                table: "SucursalGeoconfigs");

            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "Sucursal");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Sucursal");

            migrationBuilder.DropColumn(
                name: "Localidad",
                table: "Sucursal");

            migrationBuilder.DropColumn(
                name: "Provincia",
                table: "Sucursal");

            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "Sucursal");

            migrationBuilder.CreateIndex(
                name: "IX_SucursalGeoconfigs_SucursalId",
                table: "SucursalGeoconfigs",
                column: "SucursalId");
        }
    }
}
