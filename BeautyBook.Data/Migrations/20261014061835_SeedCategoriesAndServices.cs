using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BeautyBook2.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedCategoriesAndServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Фризьорство" },
                    { 2, "Маникюр" },
                    { 3, "Грим" }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "CategoryId", "Description", "DurationMinutes", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, "Професионално дамско подстригване", 45, "Дамско подстригване", 30m },
                    { 2, 1, "Боядисване на коса", 120, "Боядисване", 80m },
                    { 3, 1, "Оформяне на косата със сешоар", 30, "Сешоар", 20m },
                    { 4, 2, "Оформяне и грижа за ноктите", 45, "Класически маникюр", 25m },
                    { 5, 2, "Маникюр с гел лак", 60, "Гел лак", 35m },
                    { 6, 2, "Изграждане и оформяне на ноктите", 120, "Изграждане на нокти", 60m },
                    { 7, 3, "Лек грим за ежедневни поводи", 45, "Дневен грим", 40m },
                    { 8, 3, "Професионален грим за специални поводи", 60, "Вечерен грим", 70m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
