using Microsoft.EntityFrameworkCore.Migrations;

namespace BLG5009.Rehber.WebApp.Migrations
{
    public partial class UserStarAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Star",
                table: "Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Star",
                table: "Users");
        }
    }
}
