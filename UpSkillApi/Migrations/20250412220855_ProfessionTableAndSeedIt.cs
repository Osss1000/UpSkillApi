using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UpSkillApi.Migrations
{
    /// <inheritdoc />
    public partial class ProfessionTableAndSeedIt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Profession",
                table: "Workers");

            migrationBuilder.AddColumn<int>(
                name: "ProfessionId",
                table: "Workers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Professions",
                columns: table => new
                {
                    ProfessionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professions", x => x.ProfessionId);
                });

            migrationBuilder.InsertData(
                table: "Professions",
                columns: new[] { "ProfessionId", "Name" },
                values: new object[,]
                {
                    { 1, "نجار" },
                    { 2, "سباك" },
                    { 3, "كهربائي" },
                    { 4, "حداد" },
                    { 5, "نقاش" },
                    { 6, "تمريض" },
                    { 7, "تعليم" },
                    { 8, "أخرى" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workers_ProfessionId",
                table: "Workers",
                column: "ProfessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Professions_ProfessionId",
                table: "Workers",
                column: "ProfessionId",
                principalTable: "Professions",
                principalColumn: "ProfessionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Professions_ProfessionId",
                table: "Workers");

            migrationBuilder.DropTable(
                name: "Professions");

            migrationBuilder.DropIndex(
                name: "IX_Workers_ProfessionId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "ProfessionId",
                table: "Workers");

            migrationBuilder.AddColumn<string>(
                name: "Profession",
                table: "Workers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
