using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackKFHShortcuts.Migrations
{
    /// <inheritdoc />
    public partial class systemMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SystemMessage",
                table: "SystemMessage");

            migrationBuilder.RenameTable(
                name: "SystemMessage",
                newName: "SystemMessages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SystemMessages",
                table: "SystemMessages",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$Jz1OlnJpf/AAO3rg5HJAPeGZXwIEdiAD7D/AAMdOqgTBlquBWEH3e");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$4AVV7are2Wc2tUVUImxMXuG7J7aLl6ZDbKEX7kwXohHf6YHvHW.x6");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SystemMessages",
                table: "SystemMessages");

            migrationBuilder.RenameTable(
                name: "SystemMessages",
                newName: "SystemMessage");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SystemMessage",
                table: "SystemMessage",
                column: "Id");

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
    }
}
