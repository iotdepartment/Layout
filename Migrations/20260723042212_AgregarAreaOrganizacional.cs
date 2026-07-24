using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Layout.Migrations
{
    /// <inheritdoc />
    public partial class AgregarAreaOrganizacional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AreaOrganizacionalId",
                table: "Areas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AreaOrganizacional",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaOrganizacional", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Areas_AreaOrganizacionalId",
                table: "Areas",
                column: "AreaOrganizacionalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_AreaOrganizacional_AreaOrganizacionalId",
                table: "Areas",
                column: "AreaOrganizacionalId",
                principalTable: "AreaOrganizacional",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Areas_AreaOrganizacional_AreaOrganizacionalId",
                table: "Areas");

            migrationBuilder.DropTable(
                name: "AreaOrganizacional");

            migrationBuilder.DropIndex(
                name: "IX_Areas_AreaOrganizacionalId",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "AreaOrganizacionalId",
                table: "Areas");
        }
    }
}
