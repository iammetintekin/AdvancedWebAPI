using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WEBAPI.Migrations
{
    /// <inheritdoc />
    public partial class rolessadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
