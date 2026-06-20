using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Layout.Migrations
{
    /// <inheritdoc />
    public partial class SistemaLayout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SolicitudesMovimiento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AreaId = table.Column<int>(type: "int", nullable: false),
                    TipoMovimiento = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Razon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagenLayout = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioSolicitanteId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UsuarioAprobadorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Estatus = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaRevision = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ComentariosRevision = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudesMovimiento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolicitudesMovimiento_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolicitudesMovimiento_AspNetUsers_UsuarioAprobadorId",
                        column: x => x.UsuarioAprobadorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolicitudesMovimiento_AspNetUsers_UsuarioSolicitanteId",
                        column: x => x.UsuarioSolicitanteId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SolicitudesAprobacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolicitudMovimientoId = table.Column<int>(type: "int", nullable: false),
                    UsuarioEjecutorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FechaInicioProgramada = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaFinProgramada = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaInicioReal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaFinReal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecursosNecesarios = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstatusEjecucion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudesAprobacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolicitudesAprobacion_AspNetUsers_UsuarioEjecutorId",
                        column: x => x.UsuarioEjecutorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolicitudesAprobacion_SolicitudesMovimiento_SolicitudMovimientoId",
                        column: x => x.SolicitudMovimientoId,
                        principalTable: "SolicitudesMovimiento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SolicitudesHistorial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolicitudMovimientoId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EstatusAnterior = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstatusNuevo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comentario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudesHistorial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolicitudesHistorial_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolicitudesHistorial_SolicitudesMovimiento_SolicitudMovimientoId",
                        column: x => x.SolicitudMovimientoId,
                        principalTable: "SolicitudesMovimiento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesAprobacion_SolicitudMovimientoId",
                table: "SolicitudesAprobacion",
                column: "SolicitudMovimientoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesAprobacion_UsuarioEjecutorId",
                table: "SolicitudesAprobacion",
                column: "UsuarioEjecutorId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesHistorial_SolicitudMovimientoId",
                table: "SolicitudesHistorial",
                column: "SolicitudMovimientoId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesHistorial_UsuarioId",
                table: "SolicitudesHistorial",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesMovimiento_AreaId",
                table: "SolicitudesMovimiento",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesMovimiento_UsuarioAprobadorId",
                table: "SolicitudesMovimiento",
                column: "UsuarioAprobadorId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesMovimiento_UsuarioSolicitanteId",
                table: "SolicitudesMovimiento",
                column: "UsuarioSolicitanteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolicitudesAprobacion");

            migrationBuilder.DropTable(
                name: "SolicitudesHistorial");

            migrationBuilder.DropTable(
                name: "SolicitudesMovimiento");

            migrationBuilder.DropTable(
                name: "Areas");
        }
    }
}
