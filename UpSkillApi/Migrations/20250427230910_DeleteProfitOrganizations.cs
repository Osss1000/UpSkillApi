using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UpSkillApi.Migrations
{
    /// <inheritdoc />
    public partial class DeleteProfitOrganizations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkerApplications_PaidJobs_PaidJobId",
                table: "WorkerApplications");

            migrationBuilder.DropTable(
                name: "PaidJobs");

            migrationBuilder.DropIndex(
                name: "IX_WorkerApplications_PaidJobId",
                table: "WorkerApplications");

            migrationBuilder.DropColumn(
                name: "ApplicationType",
                table: "WorkerApplications");

            migrationBuilder.DropColumn(
                name: "PaidJobId",
                table: "WorkerApplications");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "OrganizationRole",
                table: "Organizations");

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 27, 23, 9, 9, 847, DateTimeKind.Utc).AddTicks(6520));

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 27, 23, 9, 9, 847, DateTimeKind.Utc).AddTicks(6520));

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 27, 23, 9, 9, 847, DateTimeKind.Utc).AddTicks(6520));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationType",
                table: "WorkerApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PaidJobId",
                table: "WorkerApplications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Organizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationRole",
                table: "Organizations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PaidJobs",
                columns: table => new
                {
                    PaidJobId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    PostStatusId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateAndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsManuallyClosed = table.Column<bool>(type: "bit", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NumberOfPeopleNeeded = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaidJobs", x => x.PaidJobId);
                    table.ForeignKey(
                        name: "FK_PaidJobs_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId");
                    table.ForeignKey(
                        name: "FK_PaidJobs_PostStatuses_PostStatusId",
                        column: x => x.PostStatusId,
                        principalTable: "PostStatuses",
                        principalColumn: "PostStatusId");
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_WorkerApplications_PaidJobId",
                table: "WorkerApplications",
                column: "PaidJobId");

            migrationBuilder.CreateIndex(
                name: "IX_PaidJobs_OrganizationId",
                table: "PaidJobs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_PaidJobs_PostStatusId",
                table: "PaidJobs",
                column: "PostStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerApplications_PaidJobs_PaidJobId",
                table: "WorkerApplications",
                column: "PaidJobId",
                principalTable: "PaidJobs",
                principalColumn: "PaidJobId");
        }
    }
}
