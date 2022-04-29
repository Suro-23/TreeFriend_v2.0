using Microsoft.EntityFrameworkCore.Migrations;

namespace TreeFriend.Migrations
{
    public partial class updateLectureStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Lectures");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Lectures",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Lectures");

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Lectures",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
