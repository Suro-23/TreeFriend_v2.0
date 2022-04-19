using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TreeFriend.Migrations
{
    public partial class 新增UpdateTime欄位 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "OrderDetails",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "OrderDetails");
        }
    }
}
