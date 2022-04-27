using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TreeFriend.Migrations
{
    public partial class KailinandSu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonalPost_users_UserId",
                table: "PersonalPost");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonalPost",
                table: "PersonalPost");

            migrationBuilder.DropColumn(
                name: "PostPhotoPath",
                table: "PersonalPost");

            migrationBuilder.DropColumn(
                name: "Subtitle",
                table: "PersonalPost");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "PersonalPost");

            migrationBuilder.RenameTable(
                name: "PersonalPost",
                newName: "personalPosts");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalPost_UserId",
                table: "personalPosts",
                newName: "IX_personalPosts_UserId");

            migrationBuilder.AddColumn<byte[]>(
                name: "Salt",
                table: "users",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "personalPosts",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_personalPosts",
                table: "personalPosts",
                column: "PersonalPostId");

            migrationBuilder.CreateTable(
                name: "PersonalPostImages",
                columns: table => new
                {
                    PersonalPostImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostPhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PersonalPostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalPostImages", x => x.PersonalPostImageId);
                    table.ForeignKey(
                        name: "FK_PersonalPostImages_personalPosts_PersonalPostId",
                        column: x => x.PersonalPostId,
                        principalTable: "personalPosts",
                        principalColumn: "PersonalPostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalPostMessages",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonalPostId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalPostMessages", x => x.MessageId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonalPostImages_PersonalPostId",
                table: "PersonalPostImages",
                column: "PersonalPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_personalPosts_users_UserId",
                table: "personalPosts",
                column: "UserId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_personalPosts_users_UserId",
                table: "personalPosts");

            migrationBuilder.DropTable(
                name: "PersonalPostImages");

            migrationBuilder.DropTable(
                name: "PersonalPostMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_personalPosts",
                table: "personalPosts");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "users");

            migrationBuilder.RenameTable(
                name: "personalPosts",
                newName: "PersonalPost");

            migrationBuilder.RenameIndex(
                name: "IX_personalPosts_UserId",
                table: "PersonalPost",
                newName: "IX_PersonalPost_UserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "PersonalPost",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AddColumn<string>(
                name: "PostPhotoPath",
                table: "PersonalPost",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Subtitle",
                table: "PersonalPost",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "PersonalPost",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonalPost",
                table: "PersonalPost",
                column: "PersonalPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalPost_users_UserId",
                table: "PersonalPost",
                column: "UserId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
