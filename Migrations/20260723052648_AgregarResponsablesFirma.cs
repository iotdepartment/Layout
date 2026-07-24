using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Layout.Migrations
{
    /// <inheritdoc />
    public partial class AgregarResponsablesFirma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResponsablesFirma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoFirmaId = table.Column<int>(type: "int", nullable: false),
                    AreaOrganizacionalId = table.Column<int>(type: "int", nullable: true),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponsablesFirma", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResponsablesFirma_AreasOrganizacionales_AreaOrganizacionalId",
                        column: x => x.AreaOrganizacionalId,
                        principalTable: "AreasOrganizacionales",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ResponsablesFirma_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResponsablesFirma_TiposFirma_TipoFirmaId",
                        column: x => x.TipoFirmaId,
                        principalTable: "TiposFirma",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResponsablesFirma_AreaOrganizacionalId",
                table: "ResponsablesFirma",
                column: "AreaOrganizacionalId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponsablesFirma_TipoFirmaId",
                table: "ResponsablesFirma",
                column: "TipoFirmaId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponsablesFirma_UsuarioId",
                table: "ResponsablesFirma",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResponsablesFirma");
        }
    }
}
