using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UpSkillApi.Migrations
{
    /// <inheritdoc />
    public partial class AddChatUserRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 5, 13, 15, 33, 35, 965, DateTimeKind.Utc).AddTicks(4900));

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 5, 13, 15, 33, 35, 965, DateTimeKind.Utc).AddTicks(4900));

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 5, 13, 15, 33, 35, 965, DateTimeKind.Utc).AddTicks(4900));

            migrationBuilder.CreateIndex(
                name: "IX_Chats_User1Id",
                table: "Chats",
                column: "User1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_User2Id",
                table: "Chats",
                column: "User2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Users_User1Id",
                table: "Chats",
                column: "User1Id",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Users_User2Id",
                table: "Chats",
                column: "User2Id",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Users_User1Id",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Users_User2Id",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_User1Id",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_User2Id",
                table: "Chats");

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 5, 13, 15, 5, 5, 740, DateTimeKind.Utc).AddTicks(820));

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 5, 13, 15, 5, 5, 740, DateTimeKind.Utc).AddTicks(820));

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 5, 13, 15, 5, 5, 740, DateTimeKind.Utc).AddTicks(830));
        }
    }
}
