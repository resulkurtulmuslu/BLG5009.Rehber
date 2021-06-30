using Microsoft.EntityFrameworkCore.Migrations;

namespace BLG5009.Rehber.WebApp.Migrations
{
    public partial class type : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Telephones",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Emails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Addresses",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Telephones");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Emails");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Addresses");
        }
    }
}
