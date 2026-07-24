using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Layout.Migrations
{
    /// <inheritdoc />
    public partial class AgregarAreasOrganizacionales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Areas_AreaOrganizacional_AreaOrganizacionalId",
                table: "Areas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AreaOrganizacional",
                table: "AreaOrganizacional");

            migrationBuilder.RenameTable(
                name: "AreaOrganizacional",
                newName: "AreasOrganizacionales");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AreasOrganizacionales",
                table: "AreasOrganizacionales",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_AreasOrganizacionales_AreaOrganizacionalId",
                table: "Areas",
                column: "AreaOrganizacionalId",
                principalTable: "AreasOrganizacionales",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Areas_AreasOrganizacionales_AreaOrganizacionalId",
                table: "Areas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AreasOrganizacionales",
                table: "AreasOrganizacionales");

            migrationBuilder.RenameTable(
                name: "AreasOrganizacionales",
                newName: "AreaOrganizacional");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AreaOrganizacional",
                table: "AreaOrganizacional",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_AreaOrganizacional_AreaOrganizacionalId",
                table: "Areas",
                column: "AreaOrganizacionalId",
                principalTable: "AreaOrganizacional",
                principalColumn: "Id");
        }
    }
}
