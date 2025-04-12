using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UpSkillApi.Migrations
{
    /// <inheritdoc />
    public partial class AddProfessions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Professions",
                keyColumn: "ProfessionId",
                keyValue: 6,
                column: "Name",
                value: "عامل بناء");

            migrationBuilder.UpdateData(
                table: "Professions",
                keyColumn: "ProfessionId",
                keyValue: 7,
                column: "Name",
                value: "فني رخام");

            migrationBuilder.UpdateData(
                table: "Professions",
                keyColumn: "ProfessionId",
                keyValue: 8,
                column: "Name",
                value: "فني سيراميك");

            migrationBuilder.InsertData(
                table: "Professions",
                columns: new[] { "ProfessionId", "Name" },
                values: new object[,]
                {
                    { 9, "خياطة" },
                    { 10, "سجاد يدوي" },
                    { 11, "حفر علي الخشب" },
                    { 12, "كروشيه و تريكوه" },
                    { 13, "تطريز يدوي" },
                    { 14, "اكسسوارات يدوية" },
                    { 15, "صناعة شموع" },
                    { 16, "صناعة فخار" },
                    { 17, "الرسم" },
                    { 18, "الرسم علي الزجاج" },
                    { 19, "أخرى" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Professions",
                keyColumn: "ProfessionId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Professions",
                keyColumn: "ProfessionId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Professions",
                keyColumn: "ProfessionId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Professions",
                keyColumn: "ProfessionId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Professions",
                keyColumn: "ProfessionId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Professions",
                keyColumn: "ProfessionId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Professions",
                keyColumn: "ProfessionId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Professions",
                keyColumn: "ProfessionId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Professions",
                keyColumn: "ProfessionId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Professions",
                keyColumn: "ProfessionId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Professions",
                keyColumn: "ProfessionId",
                keyValue: 19);

            migrationBuilder.UpdateData(
                table: "Professions",
                keyColumn: "ProfessionId",
                keyValue: 6,
                column: "Name",
                value: "تمريض");

            migrationBuilder.UpdateData(
                table: "Professions",
                keyColumn: "ProfessionId",
                keyValue: 7,
                column: "Name",
                value: "تعليم");

            migrationBuilder.UpdateData(
                table: "Professions",
                keyColumn: "ProfessionId",
                keyValue: 8,
                column: "Name",
                value: "أخرى");
        }
    }
}
