using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackKFHShortcuts.Migrations
{
    /// <inheritdoc />
    public partial class ProductRequestCreateAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$8w.pdYoET1yAwo4BqgLTy.QNzhiw5yzC/bEz9LLwGGOYtwM.PoN/y");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$1PH7Pcyoiq2OI86PtRQxoOG6ktoOM.7eK.zs6uaUdi5lTsNvXNGJ.");
        }
    }
}
