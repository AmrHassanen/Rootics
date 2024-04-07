using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Root.API.Migrations
{
    /// <inheritdoc />
    public partial class Update3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e3e47e1b-dbe7-49f0-83e8-6306bd59c7b5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e6aa8f27-1282-449a-8591-f6b5dd109e7d");

            migrationBuilder.DropColumn(
                name: "LastWateringDate",
                table: "UserPlantActivities");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "059c9d4f-640d-46ec-8ce1-bd23eab8979d", "1", "Admin", "ADMIN" },
                    { "9bfd70ca-3ba4-4669-ac9f-633b91c18f62", "2", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "059c9d4f-640d-46ec-8ce1-bd23eab8979d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9bfd70ca-3ba4-4669-ac9f-633b91c18f62");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastWateringDate",
                table: "UserPlantActivities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "e3e47e1b-dbe7-49f0-83e8-6306bd59c7b5", "1", "Admin", "ADMIN" },
                    { "e6aa8f27-1282-449a-8591-f6b5dd109e7d", "2", "User", "USER" }
                });
        }
    }
}
