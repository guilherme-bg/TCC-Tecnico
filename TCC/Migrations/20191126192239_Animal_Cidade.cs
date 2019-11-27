using Microsoft.EntityFrameworkCore.Migrations;

namespace TCC.Migrations
{
    public partial class Animal_Cidade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CidadeId",
                table: "Animal",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Animal_CidadeId",
                table: "Animal",
                column: "CidadeId");
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {            
        }
    }
}
