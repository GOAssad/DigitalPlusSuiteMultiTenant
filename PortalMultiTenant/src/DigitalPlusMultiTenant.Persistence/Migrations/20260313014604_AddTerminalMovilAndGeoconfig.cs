using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalPlusMultiTenant.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTerminalMovilAndGeoconfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CodigosActivacionMovil",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    LegajoId = table.Column<int>(type: "int", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaExpira = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Usado = table.Column<bool>(type: "bit", nullable: false),
                    UsadoEn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodigosActivacionMovil", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodigosActivacionMovil_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CodigosActivacionMovil_Legajo_LegajoId",
                        column: x => x.LegajoId,
                        principalTable: "Legajo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SucursalGeoconfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SucursalId = table.Column<int>(type: "int", nullable: false),
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    WifiBSSID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WifiSSID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitud = table.Column<decimal>(type: "decimal(10,7)", nullable: true),
                    Longitud = table.Column<decimal>(type: "decimal(10,7)", nullable: true),
                    RadioMetros = table.Column<int>(type: "int", nullable: false),
                    MetodoValidacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SucursalGeoconfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SucursalGeoconfigs_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SucursalGeoconfigs_Sucursal_SucursalId",
                        column: x => x.SucursalId,
                        principalTable: "Sucursal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TerminalesMoviles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    LegajoId = table.Column<int>(type: "int", nullable: false),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublicKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Plataforma = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UltimoUso = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerminalesMoviles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TerminalesMoviles_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TerminalesMoviles_Legajo_LegajoId",
                        column: x => x.LegajoId,
                        principalTable: "Legajo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CodigosActivacionMovil_EmpresaId",
                table: "CodigosActivacionMovil",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_CodigosActivacionMovil_LegajoId",
                table: "CodigosActivacionMovil",
                column: "LegajoId");

            migrationBuilder.CreateIndex(
                name: "IX_SucursalGeoconfigs_EmpresaId",
                table: "SucursalGeoconfigs",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_SucursalGeoconfigs_SucursalId",
                table: "SucursalGeoconfigs",
                column: "SucursalId");

            migrationBuilder.CreateIndex(
                name: "IX_TerminalesMoviles_EmpresaId",
                table: "TerminalesMoviles",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_TerminalesMoviles_LegajoId",
                table: "TerminalesMoviles",
                column: "LegajoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodigosActivacionMovil");

            migrationBuilder.DropTable(
                name: "SucursalGeoconfigs");

            migrationBuilder.DropTable(
                name: "TerminalesMoviles");
        }
    }
}
