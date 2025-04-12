using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UpSkillApi.Migrations
{
    /// <inheritdoc />
    public partial class completed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClientPostId",
                table: "ClientPosts",
                newName: "PostId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "ClientPosts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "ClientPosts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "ClientPosts");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "ClientPosts");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "ClientPosts",
                newName: "ClientPostId");
        }
    }
}
