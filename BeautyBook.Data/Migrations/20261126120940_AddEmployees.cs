using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BeautyBook2.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Position = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Bio", "CategoryId", "FirstName", "LastName", "Position" },
                values: new object[,]
                {
                    { 1, "Специалист по боядисване и сложни цветови техники.", 1, "Мария", "Иванова", "Стилист колорист" },
                    { 2, "Подстригване, оформяне и грижа за косата.", 1, "Десислава", "Георгиева", "Фризьор" },
                    { 3, "Класически маникюр и маникюр с гел лак.", 2, "Елена", "Петрова", "Маникюрист" },
                    { 4, "Изграждане и дизайн на нокти.", 2, "Виктория", "Стоянова", "Нокътен техник" },
                    { 5, "Дневен и ежедневен грим.", 3, "Ралица", "Димитрова", "Гримьор" },
                    { 6, "Вечерен и официален грим за специални поводи.", 3, "Калина", "Тодорова", "Визажист" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CategoryId",
                table: "Employees",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
