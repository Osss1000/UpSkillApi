using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UpSkillApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedApplicationStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ApplicationStatuses",
                columns: new[] { "ApplicationStatusId", "CreatedDate", "Description", "ModifiedDate", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 4, 13, 15, 29, 7, 171, DateTimeKind.Utc).AddTicks(4890), "Awaiting review", null, 1 },
                    { 2, new DateTime(2025, 4, 13, 15, 29, 7, 171, DateTimeKind.Utc).AddTicks(4890), "Application accepted", null, 2 },
                    { 3, new DateTime(2025, 4, 13, 15, 29, 7, 171, DateTimeKind.Utc).AddTicks(4890), "Application denied", null, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 3);
        }
    }
}
