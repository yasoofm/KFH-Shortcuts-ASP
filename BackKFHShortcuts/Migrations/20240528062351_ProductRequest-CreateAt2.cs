using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackKFHShortcuts.Migrations
{
    /// <inheritdoc />
    public partial class ProductRequestCreateAt2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "ProductRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$FkmzE5zFW1nnkLArL9JiseHLq5dgBB8yyL9QtRTVO/PuXIo3t5Q3.");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$Bz4Hq/62Jexnv9VSE9cpquolDLIx1yeDpba2zoL2XdqGddQQrVBTW");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "ProductRequests");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$HiUcBVnePpugOjFVkgaOquJtGP2GbLSp6E2T4tubWjmLb4FBVOtQu");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$rwduTq5kwaTE3X1F.qolw.guWjE8s/6biiyRYU5SBH/nXrI8ceJPq");
        }
    }
}
