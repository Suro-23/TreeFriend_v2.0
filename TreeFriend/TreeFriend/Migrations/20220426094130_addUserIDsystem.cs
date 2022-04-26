using Microsoft.EntityFrameworkCore.Migrations;

namespace TreeFriend.Migrations
{
    public partial class addUserIDsystem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SystemPost_UserId",
                table: "SystemPost",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemPost_users_UserId",
                table: "SystemPost",
                column: "UserId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemPost_users_UserId",
                table: "SystemPost");

            migrationBuilder.DropIndex(
                name: "IX_SystemPost_UserId",
                table: "SystemPost");
        }
    }
}
