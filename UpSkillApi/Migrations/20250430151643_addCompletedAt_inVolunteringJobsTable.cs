using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UpSkillApi.Migrations
{
    /// <inheritdoc />
    public partial class addCompletedAt_inVolunteringJobsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "VolunteeringJobs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 30, 15, 16, 42, 287, DateTimeKind.Utc).AddTicks(5787));

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 30, 15, 16, 42, 287, DateTimeKind.Utc).AddTicks(5791));

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 30, 15, 16, 42, 287, DateTimeKind.Utc).AddTicks(5793));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "VolunteeringJobs");

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 30, 13, 29, 30, 590, DateTimeKind.Utc).AddTicks(5670));

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 30, 13, 29, 30, 590, DateTimeKind.Utc).AddTicks(5674));

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 30, 13, 29, 30, 590, DateTimeKind.Utc).AddTicks(5675));
        }
    }
}
