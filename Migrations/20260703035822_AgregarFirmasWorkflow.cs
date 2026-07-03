using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Layout.Migrations
{
    /// <inheritdoc />
    public partial class AgregarFirmasWorkflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AreaId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoFirmaId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TiposFirma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposFirma", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SolicitudesFirma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolicitudId = table.Column<int>(type: "int", nullable: false),
                    TipoFirmaId = table.Column<int>(type: "int", nullable: false),
                    Firmada = table.Column<bool>(type: "bit", nullable: false),
                    UsuarioFirmanteId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FechaFirma = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Comentarios = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudesFirma", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolicitudesFirma_AspNetUsers_UsuarioFirmanteId",
                        column: x => x.UsuarioFirmanteId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SolicitudesFirma_SolicitudesMovimiento_SolicitudId",
                        column: x => x.SolicitudId,
                        principalTable: "SolicitudesMovimiento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolicitudesFirma_TiposFirma_TipoFirmaId",
                        column: x => x.TipoFirmaId,
                        principalTable: "TiposFirma",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AreaId",
                table: "AspNetUsers",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TipoFirmaId",
                table: "AspNetUsers",
                column: "TipoFirmaId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesFirma_SolicitudId",
                table: "SolicitudesFirma",
                column: "SolicitudId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesFirma_TipoFirmaId",
                table: "SolicitudesFirma",
                column: "TipoFirmaId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesFirma_UsuarioFirmanteId",
                table: "SolicitudesFirma",
                column: "UsuarioFirmanteId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Areas_AreaId",
                table: "AspNetUsers",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_TiposFirma_TipoFirmaId",
                table: "AspNetUsers",
                column: "TipoFirmaId",
                principalTable: "TiposFirma",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Areas_AreaId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_TiposFirma_TipoFirmaId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "SolicitudesFirma");

            migrationBuilder.DropTable(
                name: "TiposFirma");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AreaId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TipoFirmaId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TipoFirmaId",
                table: "AspNetUsers");
        }
    }
}
