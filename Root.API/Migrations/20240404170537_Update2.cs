using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Root.API.Migrations
{
    /// <inheritdoc />
    public partial class Update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "add8375c-4b0c-4485-b007-b0569ff37132");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3d1ee01-bd7b-4c61-b76b-830402a70bbc");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "e3e47e1b-dbe7-49f0-83e8-6306bd59c7b5", "1", "Admin", "ADMIN" },
                    { "e6aa8f27-1282-449a-8591-f6b5dd109e7d", "2", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e3e47e1b-dbe7-49f0-83e8-6306bd59c7b5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e6aa8f27-1282-449a-8591-f6b5dd109e7d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "add8375c-4b0c-4485-b007-b0569ff37132", "1", "Admin", "ADMIN" },
                    { "b3d1ee01-bd7b-4c61-b76b-830402a70bbc", "2", "User", "USER" }
                });
        }
    }
}
