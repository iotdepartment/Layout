using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Layout.Migrations
{
    /// <inheritdoc />
    public partial class AgregarNumeroValidacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AplicaValidacion",
                table: "SolicitudesInventario");

            migrationBuilder.DropColumn(
                name: "NumeroValidacion",
                table: "SolicitudesInventario");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaValidacion",
                table: "SolicitudesMovimiento",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroValidacion",
                table: "SolicitudesMovimiento",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaValidacion",
                table: "SolicitudesMovimiento");

            migrationBuilder.DropColumn(
                name: "NumeroValidacion",
                table: "SolicitudesMovimiento");

            migrationBuilder.AddColumn<bool>(
                name: "AplicaValidacion",
                table: "SolicitudesInventario",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NumeroValidacion",
                table: "SolicitudesInventario",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
