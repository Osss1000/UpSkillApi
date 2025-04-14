using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UpSkillApi.Migrations
{
    /// <inheritdoc />
    public partial class finalupdateISA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientPosts_Users_UserId",
                table: "ClientPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Users_UserId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Users_UserId",
                table: "Organizations");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteeringApplications_VolunteeringJobs_VolunteeringJobId",
                table: "VolunteeringApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteeringJobs_Organizations_OrganizationId",
                table: "VolunteeringJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerApplications_Workers_WorkerId",
                table: "WorkerApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Professions_ProfessionId",
                table: "Workers");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Users_UserId",
                table: "Workers");

            migrationBuilder.AddColumn<int>(
                name: "PostStatusId",
                table: "VolunteeringJobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Sponsors",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Sponsors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Sponsors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<int>(
                name: "PostStatusId",
                table: "PaidJobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PostStatusId",
                table: "ClientPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PostStatuses",
                columns: table => new
                {
                    PostStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostStatuses", x => x.PostStatusId);
                });

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 13, 18, 31, 57, 810, DateTimeKind.Utc).AddTicks(9660));

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 13, 18, 31, 57, 810, DateTimeKind.Utc).AddTicks(9660));

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 13, 18, 31, 57, 810, DateTimeKind.Utc).AddTicks(9660));

            migrationBuilder.InsertData(
                table: "PostStatuses",
                columns: new[] { "PostStatusId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "The post is publicly visible", "Posted" },
                    { 2, "The job is completed or closed", "Done" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_VolunteeringJobs_PostStatusId",
                table: "VolunteeringJobs",
                column: "PostStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PaidJobs_PostStatusId",
                table: "PaidJobs",
                column: "PostStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientPosts_PostStatusId",
                table: "ClientPosts",
                column: "PostStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientPosts_PostStatuses_PostStatusId",
                table: "ClientPosts",
                column: "PostStatusId",
                principalTable: "PostStatuses",
                principalColumn: "PostStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientPosts_Users_UserId",
                table: "ClientPosts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Users_UserId",
                table: "Clients",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Users_UserId",
                table: "Organizations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaidJobs_PostStatuses_PostStatusId",
                table: "PaidJobs",
                column: "PostStatusId",
                principalTable: "PostStatuses",
                principalColumn: "PostStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteeringApplications_VolunteeringJobs_VolunteeringJobId",
                table: "VolunteeringApplications",
                column: "VolunteeringJobId",
                principalTable: "VolunteeringJobs",
                principalColumn: "VolunteeringJobId");

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteeringJobs_Organizations_OrganizationId",
                table: "VolunteeringJobs",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteeringJobs_PostStatuses_PostStatusId",
                table: "VolunteeringJobs",
                column: "PostStatusId",
                principalTable: "PostStatuses",
                principalColumn: "PostStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerApplications_Workers_WorkerId",
                table: "WorkerApplications",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Professions_ProfessionId",
                table: "Workers",
                column: "ProfessionId",
                principalTable: "Professions",
                principalColumn: "ProfessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Users_UserId",
                table: "Workers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientPosts_PostStatuses_PostStatusId",
                table: "ClientPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientPosts_Users_UserId",
                table: "ClientPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Users_UserId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Users_UserId",
                table: "Organizations");

            migrationBuilder.DropForeignKey(
                name: "FK_PaidJobs_PostStatuses_PostStatusId",
                table: "PaidJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteeringApplications_VolunteeringJobs_VolunteeringJobId",
                table: "VolunteeringApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteeringJobs_Organizations_OrganizationId",
                table: "VolunteeringJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteeringJobs_PostStatuses_PostStatusId",
                table: "VolunteeringJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerApplications_Workers_WorkerId",
                table: "WorkerApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Professions_ProfessionId",
                table: "Workers");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Users_UserId",
                table: "Workers");

            migrationBuilder.DropTable(
                name: "PostStatuses");

            migrationBuilder.DropIndex(
                name: "IX_VolunteeringJobs_PostStatusId",
                table: "VolunteeringJobs");

            migrationBuilder.DropIndex(
                name: "IX_PaidJobs_PostStatusId",
                table: "PaidJobs");

            migrationBuilder.DropIndex(
                name: "IX_ClientPosts_PostStatusId",
                table: "ClientPosts");

            migrationBuilder.DropColumn(
                name: "PostStatusId",
                table: "VolunteeringJobs");

            migrationBuilder.DropColumn(
                name: "PostStatusId",
                table: "PaidJobs");

            migrationBuilder.DropColumn(
                name: "PostStatusId",
                table: "ClientPosts");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Sponsors",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Sponsors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Sponsors",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 13, 15, 29, 7, 171, DateTimeKind.Utc).AddTicks(4890));

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 13, 15, 29, 7, 171, DateTimeKind.Utc).AddTicks(4890));

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "ApplicationStatusId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 13, 15, 29, 7, 171, DateTimeKind.Utc).AddTicks(4890));

            migrationBuilder.AddForeignKey(
                name: "FK_ClientPosts_Users_UserId",
                table: "ClientPosts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Users_UserId",
                table: "Clients",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Users_UserId",
                table: "Organizations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteeringApplications_VolunteeringJobs_VolunteeringJobId",
                table: "VolunteeringApplications",
                column: "VolunteeringJobId",
                principalTable: "VolunteeringJobs",
                principalColumn: "VolunteeringJobId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteeringJobs_Organizations_OrganizationId",
                table: "VolunteeringJobs",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "OrganizationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerApplications_Workers_WorkerId",
                table: "WorkerApplications",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "WorkerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Professions_ProfessionId",
                table: "Workers",
                column: "ProfessionId",
                principalTable: "Professions",
                principalColumn: "ProfessionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Users_UserId",
                table: "Workers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
