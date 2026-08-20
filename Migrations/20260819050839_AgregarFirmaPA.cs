using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Layout.Migrations
{
    /// <inheritdoc />
    public partial class AgregarFirmaPA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FirmasPA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioTitularId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UsuarioPAId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TipoFirmaId = table.Column<int>(type: "int", nullable: false),
                    MotivoAsignacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAsignacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FirmasPA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FirmasPA_AspNetUsers_UsuarioPAId",
                        column: x => x.UsuarioPAId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FirmasPA_AspNetUsers_UsuarioTitularId",
                        column: x => x.UsuarioTitularId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FirmasPA_TiposFirma_TipoFirmaId",
                        column: x => x.TipoFirmaId,
                        principalTable: "TiposFirma",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FirmasPA_TipoFirmaId",
                table: "FirmasPA",
                column: "TipoFirmaId");

            migrationBuilder.CreateIndex(
                name: "IX_FirmasPA_UsuarioPAId",
                table: "FirmasPA",
                column: "UsuarioPAId");

            migrationBuilder.CreateIndex(
                name: "IX_FirmasPA_UsuarioTitularId",
                table: "FirmasPA",
                column: "UsuarioTitularId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FirmasPA");
        }
    }
}
