using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Layout.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelacionInventarioYFolio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Folio",
                table: "SolicitudesMovimiento",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SolicitudesInventario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolicitudId = table.Column<int>(type: "int", nullable: false),
                    AplicaValidacion = table.Column<bool>(type: "bit", nullable: false),
                    NumeroValidacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AplicaResponsable = table.Column<bool>(type: "bit", nullable: false),
                    ResponsableInventario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AplicaMandril = table.Column<bool>(type: "bit", nullable: false),
                    MandrilKanbanNP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AplicaPallets = table.Column<bool>(type: "bit", nullable: false),
                    NumeroPallets = table.Column<int>(type: "int", nullable: true),
                    AplicaRazon = table.Column<bool>(type: "bit", nullable: false),
                    RazonInventario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AplicaFecha = table.Column<bool>(type: "bit", nullable: false),
                    FechaCompromiso = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudesInventario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolicitudesInventario_SolicitudesMovimiento_SolicitudId",
                        column: x => x.SolicitudId,
                        principalTable: "SolicitudesMovimiento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesMovimiento_Folio",
                table: "SolicitudesMovimiento",
                column: "Folio",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesInventario_SolicitudId",
                table: "SolicitudesInventario",
                column: "SolicitudId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolicitudesInventario");

            migrationBuilder.DropIndex(
                name: "IX_SolicitudesMovimiento_Folio",
                table: "SolicitudesMovimiento");

            migrationBuilder.DropColumn(
                name: "Folio",
                table: "SolicitudesMovimiento");
        }
    }
}
