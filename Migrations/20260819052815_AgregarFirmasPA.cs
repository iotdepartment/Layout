using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Layout.Migrations
{
    /// <inheritdoc />
    public partial class AgregarFirmasPA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsFirmaPA",
                table: "SolicitudesFirma",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MotivoFirmaPA",
                table: "SolicitudesFirma",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioPAId",
                table: "SolicitudesFirma",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesFirma_UsuarioPAId",
                table: "SolicitudesFirma",
                column: "UsuarioPAId");

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitudesFirma_AspNetUsers_UsuarioPAId",
                table: "SolicitudesFirma",
                column: "UsuarioPAId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SolicitudesFirma_AspNetUsers_UsuarioPAId",
                table: "SolicitudesFirma");

            migrationBuilder.DropIndex(
                name: "IX_SolicitudesFirma_UsuarioPAId",
                table: "SolicitudesFirma");

            migrationBuilder.DropColumn(
                name: "EsFirmaPA",
                table: "SolicitudesFirma");

            migrationBuilder.DropColumn(
                name: "MotivoFirmaPA",
                table: "SolicitudesFirma");

            migrationBuilder.DropColumn(
                name: "UsuarioPAId",
                table: "SolicitudesFirma");
        }
    }
}
