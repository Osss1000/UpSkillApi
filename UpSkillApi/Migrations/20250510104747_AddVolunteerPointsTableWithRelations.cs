using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UpSkillApi.Migrations
{
    /// <inheritdoc />
    public partial class AddVolunteerPointsTableWithRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VolunteerPoints",
                columns: table => new
                {
                    VolunteerPointsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    VolunteeringJobId = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    AwardedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteerPoints", x => x.VolunteerPointsId);
                    table.ForeignKey(
                        name: "FK_VolunteerPoints_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_VolunteerPoints_VolunteeringJobs_VolunteeringJobId",
                        column: x => x.VolunteeringJobId,
                        principalTable: "VolunteeringJobs",
                        principalColumn: "VolunteeringJobId");
                });

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 5, 10, 10, 47, 47, 70, DateTimeKind.Utc).AddTicks(9070));

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 5, 10, 10, 47, 47, 70, DateTimeKind.Utc).AddTicks(9070));

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 5, 10, 10, 47, 47, 70, DateTimeKind.Utc).AddTicks(9080));

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerPoints_UserId",
                table: "VolunteerPoints",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerPoints_VolunteeringJobId",
                table: "VolunteerPoints",
                column: "VolunteeringJobId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VolunteerPoints");

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 5, 10, 4, 56, 52, 176, DateTimeKind.Utc).AddTicks(6020));

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 5, 10, 4, 56, 52, 176, DateTimeKind.Utc).AddTicks(6020));

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 5, 10, 4, 56, 52, 176, DateTimeKind.Utc).AddTicks(6030));
        }
    }
}
