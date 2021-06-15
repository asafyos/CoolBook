using Microsoft.EntityFrameworkCore.Migrations;

namespace CoolBook.Migrations
{
    public partial class Updateuserinfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "UserInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "UserInfo");
        }
    }
}
