using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Layout.Migrations
{
    /// <inheritdoc />
    public partial class RelacionUsuariosAreas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Areas_AreaId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_TiposFirma_TipoFirmaId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_SolicitudesFirma_AspNetUsers_UsuarioFirmanteId",
                table: "SolicitudesFirma");

            migrationBuilder.DropForeignKey(
                name: "FK_SolicitudesFirma_TiposFirma_TipoFirmaId",
                table: "SolicitudesFirma");

            migrationBuilder.DropForeignKey(
                name: "FK_SolicitudMovimientosTecnicos_SolicitudesMovimiento_SolicitudId",
                table: "SolicitudMovimientosTecnicos");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AreaId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SolicitudMovimientosTecnicos",
                table: "SolicitudMovimientosTecnicos");

            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "SolicitudMovimientosTecnicos",
                newName: "SolicitudesMovimientosTecnicos");

            migrationBuilder.RenameIndex(
                name: "IX_SolicitudMovimientosTecnicos_SolicitudId",
                table: "SolicitudesMovimientosTecnicos",
                newName: "IX_SolicitudesMovimientosTecnicos_SolicitudId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SolicitudesMovimientosTecnicos",
                table: "SolicitudesMovimientosTecnicos",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UsuarioAreas",
                columns: table => new
                {
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AreaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioAreas", x => new { x.UsuarioId, x.AreaId });
                    table.ForeignKey(
                        name: "FK_UsuarioAreas_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioAreas_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioAreas_AreaId",
                table: "UsuarioAreas",
                column: "AreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_TiposFirma_TipoFirmaId",
                table: "AspNetUsers",
                column: "TipoFirmaId",
                principalTable: "TiposFirma",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitudesFirma_AspNetUsers_UsuarioFirmanteId",
                table: "SolicitudesFirma",
                column: "UsuarioFirmanteId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitudesFirma_TiposFirma_TipoFirmaId",
                table: "SolicitudesFirma",
                column: "TipoFirmaId",
                principalTable: "TiposFirma",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitudesMovimientosTecnicos_SolicitudesMovimiento_SolicitudId",
                table: "SolicitudesMovimientosTecnicos",
                column: "SolicitudId",
                principalTable: "SolicitudesMovimiento",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_TiposFirma_TipoFirmaId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_SolicitudesFirma_AspNetUsers_UsuarioFirmanteId",
                table: "SolicitudesFirma");

            migrationBuilder.DropForeignKey(
                name: "FK_SolicitudesFirma_TiposFirma_TipoFirmaId",
                table: "SolicitudesFirma");

            migrationBuilder.DropForeignKey(
                name: "FK_SolicitudesMovimientosTecnicos_SolicitudesMovimiento_SolicitudId",
                table: "SolicitudesMovimientosTecnicos");

            migrationBuilder.DropTable(
                name: "UsuarioAreas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SolicitudesMovimientosTecnicos",
                table: "SolicitudesMovimientosTecnicos");

            migrationBuilder.RenameTable(
                name: "SolicitudesMovimientosTecnicos",
                newName: "SolicitudMovimientosTecnicos");

            migrationBuilder.RenameIndex(
                name: "IX_SolicitudesMovimientosTecnicos_SolicitudId",
                table: "SolicitudMovimientosTecnicos",
                newName: "IX_SolicitudMovimientosTecnicos_SolicitudId");

            migrationBuilder.AddColumn<int>(
                name: "AreaId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SolicitudMovimientosTecnicos",
                table: "SolicitudMovimientosTecnicos",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AreaId",
                table: "AspNetUsers",
                column: "AreaId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitudesFirma_AspNetUsers_UsuarioFirmanteId",
                table: "SolicitudesFirma",
                column: "UsuarioFirmanteId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitudesFirma_TiposFirma_TipoFirmaId",
                table: "SolicitudesFirma",
                column: "TipoFirmaId",
                principalTable: "TiposFirma",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitudMovimientosTecnicos_SolicitudesMovimiento_SolicitudId",
                table: "SolicitudMovimientosTecnicos",
                column: "SolicitudId",
                principalTable: "SolicitudesMovimiento",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
