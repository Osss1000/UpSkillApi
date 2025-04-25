using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UpSkillApi.Migrations
{
    /// <inheritdoc />
    public partial class ooiiee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientPosts_Users_UserId",
                table: "ClientPosts");

            migrationBuilder.DropIndex(
                name: "IX_ClientPosts_UserId",
                table: "ClientPosts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ClientPosts");

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 22, 18, 53, 46, 144, DateTimeKind.Utc).AddTicks(1830));

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 22, 18, 53, 46, 144, DateTimeKind.Utc).AddTicks(1840));

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 22, 18, 53, 46, 144, DateTimeKind.Utc).AddTicks(1840));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ClientPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 22, 0, 42, 30, 334, DateTimeKind.Utc).AddTicks(5680));

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 22, 0, 42, 30, 334, DateTimeKind.Utc).AddTicks(5680));

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 22, 0, 42, 30, 334, DateTimeKind.Utc).AddTicks(5680));

            migrationBuilder.CreateIndex(
                name: "IX_ClientPosts_UserId",
                table: "ClientPosts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientPosts_Users_UserId",
                table: "ClientPosts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
