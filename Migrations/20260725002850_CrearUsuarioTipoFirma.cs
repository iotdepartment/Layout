using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Layout.Migrations
{
    /// <inheritdoc />
    public partial class CrearUsuarioTipoFirma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_TiposFirma_TipoFirmaId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TipoFirmaId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TipoFirmaId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "UsuarioTiposFirma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TipoFirmaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioTiposFirma", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioTiposFirma_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioTiposFirma_TiposFirma_TipoFirmaId",
                        column: x => x.TipoFirmaId,
                        principalTable: "TiposFirma",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioTiposFirma_TipoFirmaId",
                table: "UsuarioTiposFirma",
                column: "TipoFirmaId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioTiposFirma_UsuarioId",
                table: "UsuarioTiposFirma",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuarioTiposFirma");

            migrationBuilder.AddColumn<int>(
                name: "TipoFirmaId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TipoFirmaId",
                table: "AspNetUsers",
                column: "TipoFirmaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_TiposFirma_TipoFirmaId",
                table: "AspNetUsers",
                column: "TipoFirmaId",
                principalTable: "TiposFirma",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
