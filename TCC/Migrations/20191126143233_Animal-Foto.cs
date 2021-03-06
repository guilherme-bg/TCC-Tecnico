﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TCC.Migrations {
    public partial class AnimalFoto : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {


            migrationBuilder.CreateTable(
                name: "Animal",
                columns: table => new {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Foto1 = table.Column<string>(nullable: false),
                    Foto2 = table.Column<string>(nullable: true),
                    Foto3 = table.Column<string>(nullable: true),
                    Nome = table.Column<string>(nullable: false),
                    Especie = table.Column<string>(nullable: false),
                    Sexo = table.Column<string>(nullable: false),
                    Porte = table.Column<string>(nullable: false),
                    Saude = table.Column<string>(nullable: true),
                    Vacina = table.Column<string>(nullable: true),
                    Descricao = table.Column<string>(nullable: false),
                    Obs = table.Column<string>(nullable: true),
                    Data_Cadastro = table.Column<DateTime>(nullable: false),
                    UsuarioId = table.Column<string>(nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Animal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Animal_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Animal_UsuarioId",
                table: "Animal",
                column: "UsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder) {

        }
    }
}
