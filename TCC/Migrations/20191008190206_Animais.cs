using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TCC.Migrations
{
    public partial class Animais : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Animal",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(nullable: false),
                    Especie = table.Column<string>(nullable: false),
                    Sexo = table.Column<string>(nullable: false),
                    Porte = table.Column<string>(nullable: false),
                    Saude = table.Column<string>(nullable: false),
                    Descricao = table.Column<string>(nullable: false),
                    Data_Cadastro = table.Column<DateTime>(nullable: false),
                    UsuarioId1 = table.Column<string>(nullable: true),
                    UsuarioId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Animal_AspNetUsers_UsuarioId1",
                        column: x => x.UsuarioId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Animal_UsuarioId1",
                table: "Animal",
                column: "UsuarioId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Animal");
        }
    }
}
