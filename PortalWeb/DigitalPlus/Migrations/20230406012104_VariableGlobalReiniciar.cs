using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalPlus.Migrations
{
    /// <inheritdoc />
    public partial class VariableGlobalReiniciar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Reiniciar",
                table: "VariablesGlobales",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reiniciar",
                table: "VariablesGlobales");
        }
    }
}
