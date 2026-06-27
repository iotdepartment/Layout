using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Layout.Migrations
{
    /// <inheritdoc />
    public partial class AddMovimientosTecnicos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SolicitudMovimientosTecnicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolicitudId = table.Column<int>(type: "int", nullable: false),
                    MovimientoRed = table.Column<bool>(type: "bit", nullable: false),
                    MovimientoIoT = table.Column<bool>(type: "bit", nullable: false),
                    MovimientoProgramacion = table.Column<bool>(type: "bit", nullable: false),
                    MovimientoElectrico = table.Column<bool>(type: "bit", nullable: false),
                    MovimientoEHS = table.Column<bool>(type: "bit", nullable: false),
                    RequierePCR = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudMovimientosTecnicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolicitudMovimientosTecnicos_SolicitudesMovimiento_SolicitudId",
                        column: x => x.SolicitudId,
                        principalTable: "SolicitudesMovimiento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudMovimientosTecnicos_SolicitudId",
                table: "SolicitudMovimientosTecnicos",
                column: "SolicitudId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolicitudMovimientosTecnicos");
        }
    }
}
