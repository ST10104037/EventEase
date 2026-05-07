using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEase.Migrations
{
    /// <inheritdoc />
    public partial class MakeContactFieldsNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Venues",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ContactPhone",
                table: "Venues",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ContactEmail",
                table: "Venues",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id-constant",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "87b63975-e48a-4247-9322-21db32c044cf", "AQAAAAIAAYagAAAAEJhs4B7qXAIjoviqm7ddlM1HKLWLUQNOu7OQQWaFXX+x/PsnWH/Mh5pl9xtxHTkbBQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "specialist-user-id-constant",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "34ebfc97-e6eb-4ce3-8571-8e354b210a1b", "AQAAAAIAAYagAAAAEJpINYoSC2vNcyy5m+hiGm5NdnAJBqX5QvAvMHFJ0Sp2Fl6I6/V39DeBj3zk/9B4nw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Venues",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContactPhone",
                table: "Venues",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContactEmail",
                table: "Venues",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id-constant",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "02626a3f-033f-4334-a43a-a870c97af6b4", "AQAAAAIAAYagAAAAEPVECTQiAImt/1pH40iBe7Y63eDbcun2M37oc5gvXQAKMqbFisyt1KwM1hVlFGZl7w==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "specialist-user-id-constant",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f4209278-2cd1-471c-8392-868496797b7d", "AQAAAAIAAYagAAAAEArnoIbNL2LBxnOzfnOt8mSFGtRZvakrz7asINB3VweBjQmYXPQc1Tj1x7hvUzXGLw==" });
        }
    }
}
