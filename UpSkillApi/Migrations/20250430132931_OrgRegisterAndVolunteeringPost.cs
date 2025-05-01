using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UpSkillApi.Migrations
{
    /// <inheritdoc />
    public partial class OrgRegisterAndVolunteeringPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CommercialRecordPath",
                table: "Organizations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommercialRecordPath",
                table: "Organizations");

           
        }
    }
}
