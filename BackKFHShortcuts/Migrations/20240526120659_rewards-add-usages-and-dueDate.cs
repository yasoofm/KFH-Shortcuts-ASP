using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackKFHShortcuts.Migrations
{
    /// <inheritdoc />
    public partial class rewardsaddusagesanddueDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "Rewards",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Usages",
                table: "Rewards",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Rewards");

            migrationBuilder.DropColumn(
                name: "Usages",
                table: "Rewards");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$/koF6JpaaLjDehxiWfO7XemynqMp4QkIL2eLZWzfIcUuo2PSWh0Di");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$QxYsUM6kTduGM/7ArVBN0.QIO8zQKdwwZVjSihZvR.FSVpC4CMysS");
        }
    }
}
