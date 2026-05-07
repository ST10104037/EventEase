using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EventEase.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsersAndRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "admin-user-id-constant", 0, "02626a3f-033f-4334-a43a-a870c97af6b4", "admin@eventease.com", true, false, null, "ADMIN@EVENTEASE.COM", "ADMIN@EVENTEASE.COM", "AQAAAAIAAYagAAAAEPVECTQiAImt/1pH40iBe7Y63eDbcun2M37oc5gvXQAKMqbFisyt1KwM1hVlFGZl7w==", null, false, "", false, "admin@eventease.com" },
                    { "specialist-user-id-constant", 0, "f4209278-2cd1-471c-8392-868496797b7d", "specialist@eventease.com", true, false, null, "SPECIALIST@EVENTEASE.COM", "SPECIALIST@EVENTEASE.COM", "AQAAAAIAAYagAAAAEArnoIbNL2LBxnOzfnOt8mSFGtRZvakrz7asINB3VweBjQmYXPQc1Tj1x7hvUzXGLw==", null, false, "", false, "specialist@eventease.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "admin-role-id-constant", "admin-user-id-constant" },
                    { "specialist-role-id-constant", "specialist-user-id-constant" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "admin-role-id-constant", "admin-user-id-constant" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "specialist-role-id-constant", "specialist-user-id-constant" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id-constant");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "specialist-user-id-constant");
        }
    }
}
