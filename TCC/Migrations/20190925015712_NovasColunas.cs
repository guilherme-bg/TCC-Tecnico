using Microsoft.EntityFrameworkCore.Migrations;

namespace TCC.Migrations
{
    public partial class NovasColunas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Moradia",
                table: "Usuario",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Protecao",
                table: "Usuario",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QtAnimais",
                table: "Usuario",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Moradia",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Protecao",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "QtAnimais",
                table: "Usuario");
        }
    }
}
