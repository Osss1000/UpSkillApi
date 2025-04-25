using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UpSkillApi.Migrations
{
    /// <inheritdoc />
    public partial class newone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VolunteeringJobs_ApplicationStatuses_ApplicationStatusId",
                table: "VolunteeringJobs");

            migrationBuilder.DropIndex(
                name: "IX_VolunteeringJobs_ApplicationStatusId",
                table: "VolunteeringJobs");

            migrationBuilder.DropColumn(
                name: "ApplicationStatusId",
                table: "VolunteeringJobs");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationStatusId",
                table: "VolunteeringJobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 21, 1, 53, 33, 590, DateTimeKind.Utc).AddTicks(8860));

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 21, 1, 53, 33, 590, DateTimeKind.Utc).AddTicks(8860));

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 21, 1, 53, 33, 590, DateTimeKind.Utc).AddTicks(8880));

            migrationBuilder.CreateIndex(
                name: "IX_VolunteeringJobs_ApplicationStatusId",
                table: "VolunteeringJobs",
                column: "ApplicationStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteeringJobs_ApplicationStatuses_ApplicationStatusId",
                table: "VolunteeringJobs",
                column: "ApplicationStatusId",
                principalTable: "ApplicationStatuses",
                principalColumn: "ApplicationStatusId");
        }
    }
}
