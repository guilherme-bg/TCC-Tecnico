using Microsoft.EntityFrameworkCore.Migrations;

namespace TCC.Migrations
{
    public partial class Adotado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {            
            migrationBuilder.AddColumn<bool>(
                name: "Adotado",
                table: "Animal",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {            
        }
    }
}
