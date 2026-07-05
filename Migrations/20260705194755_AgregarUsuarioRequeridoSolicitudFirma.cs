using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Layout.Migrations
{
    /// <inheritdoc />
    public partial class AgregarUsuarioRequeridoSolicitudFirma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioRequeridoId",
                table: "SolicitudesFirma",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesFirma_UsuarioRequeridoId",
                table: "SolicitudesFirma",
                column: "UsuarioRequeridoId");

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitudesFirma_AspNetUsers_UsuarioRequeridoId",
                table: "SolicitudesFirma",
                column: "UsuarioRequeridoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SolicitudesFirma_AspNetUsers_UsuarioRequeridoId",
                table: "SolicitudesFirma");

            migrationBuilder.DropIndex(
                name: "IX_SolicitudesFirma_UsuarioRequeridoId",
                table: "SolicitudesFirma");

            migrationBuilder.DropColumn(
                name: "UsuarioRequeridoId",
                table: "SolicitudesFirma");
        }
    }
}
