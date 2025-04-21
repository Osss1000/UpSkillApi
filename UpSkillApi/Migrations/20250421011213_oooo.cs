using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UpSkillApi.Migrations
{
    /// <inheritdoc />
    public partial class oooo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicantType",
                table: "VolunteeringApplications");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "ClientPosts");

            migrationBuilder.DropColumn(
                name: "Profession",
                table: "ClientPosts");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationType",
                table: "WorkerApplications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "ProfessionId",
                table: "ClientPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 21, 1, 12, 13, 657, DateTimeKind.Utc).AddTicks(3710));

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 21, 1, 12, 13, 657, DateTimeKind.Utc).AddTicks(3710));

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 21, 1, 12, 13, 657, DateTimeKind.Utc).AddTicks(3710));

            migrationBuilder.CreateIndex(
                name: "IX_ClientPosts_ProfessionId",
                table: "ClientPosts",
                column: "ProfessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientPosts_Professions_ProfessionId",
                table: "ClientPosts",
                column: "ProfessionId",
                principalTable: "Professions",
                principalColumn: "ProfessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientPosts_Professions_ProfessionId",
                table: "ClientPosts");

            migrationBuilder.DropIndex(
                name: "IX_ClientPosts_ProfessionId",
                table: "ClientPosts");

            migrationBuilder.DropColumn(
                name: "ProfessionId",
                table: "ClientPosts");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationType",
                table: "WorkerApplications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ApplicantType",
                table: "VolunteeringApplications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "ClientPosts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Profession",
                table: "ClientPosts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 19, 2, 59, 33, 280, DateTimeKind.Utc).AddTicks(5140));

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 19, 2, 59, 33, 280, DateTimeKind.Utc).AddTicks(5140));

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 19, 2, 59, 33, 280, DateTimeKind.Utc).AddTicks(5140));
        }
    }
}
