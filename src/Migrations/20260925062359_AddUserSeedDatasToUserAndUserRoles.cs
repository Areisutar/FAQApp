using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace src.Migrations
{
    /// <inheritdoc />
    public partial class AddUserSeedDatasToUserAndUserRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "79a68294-7d70-4ed6-b153-66b7e6dcad50", "626edaf5-52c7-4d5a-ae52-67288ac88c22", "Admin", "ADMIN" },
                    { "a866201e-95db-46c5-9c48-f61b65ebd43b", "8f98ae83-c50c-4fa4-a2e7-1f3b194522ab", "Public", "PUBLIC" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "6e690231-d710-45a0-bc37-ea51a6e108a9", 0, "0cbe2fc1-b1dc-4e3d-9d32-14fb5edfdcf6", "public@example.com", true, true, null, "PUBLIC@EXAMPLE.COM", "PUBLIC@EXAMPLE.COM", "AQAAAAIAAYagAAAAEHC70Ils7gTpKze7jnZKO6K109VyYE4qLZMb18aClPbSW4N1Vlu7bkml93DcT2sx0g==", null, false, "e47200f6-2a89-4821-a949-05e6798513b3", false, "public@example.com" },
                    { "ad9bb655-720e-477e-b7a5-cb8dc3a7dc2d", 0, "5aa4c359-7ca1-4575-b573-cf5c2e35d37c", "admin@example.com", true, true, null, "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAELMa5Q7gWV1bKUHS38wrenU5clMlGZ5ggzBNhtcSdyhcEyPmoQzEWnceLGscaAG2ew==", null, false, "969821ba-29b6-4767-9d64-552ece616b16", false, "admin@example.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "a866201e-95db-46c5-9c48-f61b65ebd43b", "6e690231-d710-45a0-bc37-ea51a6e108a9" },
                    { "79a68294-7d70-4ed6-b153-66b7e6dcad50", "ad9bb655-720e-477e-b7a5-cb8dc3a7dc2d" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "a866201e-95db-46c5-9c48-f61b65ebd43b", "6e690231-d710-45a0-bc37-ea51a6e108a9" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "79a68294-7d70-4ed6-b153-66b7e6dcad50", "ad9bb655-720e-477e-b7a5-cb8dc3a7dc2d" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "79a68294-7d70-4ed6-b153-66b7e6dcad50");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a866201e-95db-46c5-9c48-f61b65ebd43b");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6e690231-d710-45a0-bc37-ea51a6e108a9");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ad9bb655-720e-477e-b7a5-cb8dc3a7dc2d");
        }
    }
}
