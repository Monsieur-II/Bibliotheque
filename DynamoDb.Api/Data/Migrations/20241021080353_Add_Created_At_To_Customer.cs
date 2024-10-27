using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DynamoDb.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Created_At_To_Customer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: "9ff725fee0954fe39ab2f8e131bc153c");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Customers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "CreatedAt", "Email", "IsDeleted", "Name", "PhoneNumber" },
                values: new object[] { "a277044b864241cd868c0f7cb1c8f6dd", new DateTime(2024, 10, 21, 8, 3, 53, 472, DateTimeKind.Utc).AddTicks(4250), null, false, "John Doe", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: "a277044b864241cd868c0f7cb1c8f6dd");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Customers");

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Email", "IsDeleted", "Name", "PhoneNumber" },
                values: new object[] { "9ff725fee0954fe39ab2f8e131bc153c", null, false, "John Doe", null });
        }
    }
}
