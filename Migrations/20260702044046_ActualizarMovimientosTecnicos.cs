using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Layout.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarMovimientosTecnicos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MovimientoIoT",
                table: "SolicitudMovimientosTecnicos");

            migrationBuilder.RenameColumn(
                name: "MovimientoRed",
                table: "SolicitudMovimientosTecnicos",
                newName: "MovimientoITIoT");

            migrationBuilder.AddColumn<string>(
                name: "NumeroPCR",
                table: "SolicitudMovimientosTecnicos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroPCR",
                table: "SolicitudMovimientosTecnicos");

            migrationBuilder.RenameColumn(
                name: "MovimientoITIoT",
                table: "SolicitudMovimientosTecnicos",
                newName: "MovimientoRed");

            migrationBuilder.AddColumn<bool>(
                name: "MovimientoIoT",
                table: "SolicitudMovimientosTecnicos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
