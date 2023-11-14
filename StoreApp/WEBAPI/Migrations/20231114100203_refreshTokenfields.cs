using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WEBAPI.Migrations
{
    /// <inheritdoc />
    public partial class refreshTokenfields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4598264a-b4e4-4de8-a47b-c236bd7118ee");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6734505b-016c-49de-b2e7-1929f6a278be");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "77ed5ce8-442f-4164-954a-58e48d0285fb");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpireTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2e7fbe25-4768-456c-b21f-ce07a3449d14", null, "User", "USER" },
                    { "9c27419c-2dca-4db7-8e47-261960d1dc4a", null, "Admin", "ADMIN" },
                    { "a4e60a64-1669-4f5c-9d0e-b8a00069776d", null, "Personel", "PERSONEL" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2e7fbe25-4768-456c-b21f-ce07a3449d14");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9c27419c-2dca-4db7-8e47-261960d1dc4a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a4e60a64-1669-4f5c-9d0e-b8a00069776d");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpireTime",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4598264a-b4e4-4de8-a47b-c236bd7118ee", null, "Admin", "ADMIN" },
                    { "6734505b-016c-49de-b2e7-1929f6a278be", null, "User", "USER" },
                    { "77ed5ce8-442f-4164-954a-58e48d0285fb", null, "Personel", "PERSONEL" }
                });
        }
    }
}
