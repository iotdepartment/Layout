using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Layout.Migrations
{
    /// <inheritdoc />
    public partial class AgregarImagenesMovimientoTecnico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagenAntes",
                table: "SolicitudMovimientosTecnicos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagenDespues",
                table: "SolicitudMovimientosTecnicos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagenAntes",
                table: "SolicitudMovimientosTecnicos");

            migrationBuilder.DropColumn(
                name: "ImagenDespues",
                table: "SolicitudMovimientosTecnicos");
        }
    }
}
