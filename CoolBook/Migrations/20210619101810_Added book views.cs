using Microsoft.EntityFrameworkCore.Migrations;

namespace CoolBook.Migrations
{
    public partial class Addedbookviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Views",
                table: "Book",
                type: "decimal(20,0)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Views",
                table: "Book");
        }
    }
}
