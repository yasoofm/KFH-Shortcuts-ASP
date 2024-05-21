using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackKFHShortcuts.Migrations
{
    /// <inheritdoc />
    public partial class AddDummyUserAccounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "IsAdmin", "KFH_Id", "LastName", "Password", "Points" },
                values: new object[,]
                {
                    { 1, "admin@mail.com", "Yousef", true, 1111, "Mandani", "$2a$11$ZsfCaUxsPJG2oOfvlqcKpOyFlSL9CPotWH8qg8Qkk.PnAAmZv6sKu", 0 },
                    { 2, "user@mail.com", "Yousef", false, 1234, "Mandani", "$2a$11$cba2aVBYp4MYvfwi96OQfecJYfu2bN79VBYHWBZoW.sWInaonQisO", 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
