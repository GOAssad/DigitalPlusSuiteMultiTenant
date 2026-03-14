using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalPlusMultiTenant.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMobileHabilitado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MobileHabilitado",
                table: "Legajo",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MobileHabilitado",
                table: "Empresa",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MobileHabilitado",
                table: "Legajo");

            migrationBuilder.DropColumn(
                name: "MobileHabilitado",
                table: "Empresa");
        }
    }
}
