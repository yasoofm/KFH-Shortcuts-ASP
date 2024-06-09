using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackKFHShortcuts.Migrations
{
    /// <inheritdoc />
    public partial class systemmessagesmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SystemMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemMessage", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "SystemMessage",
                columns: new[] { "Id", "Content", "Role" },
                values: new object[,]
                {
                    { 1, "If asked about Prepaid Cards reply with Oasis Club Mastercard World, Hesabi, Al-Osra, and Foreign Currency", "system" },
                    { 2, "If asked about Financial Investments reply with Al Nuwair, Al Sidra, Al Dima, and Al Khomasiyah", "system" },
                    { 3, "If asked about Accounts reply with Alrabeh and Investment saving account", "system" },
                    { 4, "If asked about Credit Cards reply with Oasis Club Mastercard World and Visa Diamond", "system" },
                    { 5, "You are a chatbot used to help KFH Employees promote and sell KFH products", "system" },
                    { 6, "make your replies as short and concise as possible while giving the asked for information in a clear way", "system" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$PAT5cVIRTHA1PIYpSvKVUuBBG.X9jdPNSdQGJ6VJfuO.BJvjnq6ia");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$jobxxfPBoZeNh7dzVOqRM.v.INCPEi/V.jZM/82xFxHQUZSU/zn3q");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemMessage");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$3LIB5HAwNskT2GTqw4dR1eOsmpR9fM8pOP5crGdXo/gvkfIqDcEpa");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$881uVRduEz1RyGu6Zxlikebb7pnvx/QRnHXBCEyEWGsUeg8AC6laC");
        }
    }
}
