using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Layout.Migrations
{
    /// <inheritdoc />
    public partial class MoverFechasASolicitudMovimiento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AplicaFecha",
                table: "SolicitudesInventario");

            migrationBuilder.DropColumn(
                name: "FechaCompromiso",
                table: "SolicitudesInventario");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaFinMovimiento",
                table: "SolicitudesMovimiento",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaInicioMovimiento",
                table: "SolicitudesMovimiento",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaFinMovimiento",
                table: "SolicitudesMovimiento");

            migrationBuilder.DropColumn(
                name: "FechaInicioMovimiento",
                table: "SolicitudesMovimiento");

            migrationBuilder.AddColumn<bool>(
                name: "AplicaFecha",
                table: "SolicitudesInventario",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCompromiso",
                table: "SolicitudesInventario",
                type: "datetime2",
                nullable: true);
        }
    }
}
