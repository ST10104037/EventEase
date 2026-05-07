using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEase.Migrations
{
    /// <inheritdoc />
    public partial class MakeEventFieldsNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id-constant",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "629350b1-b681-4b8a-bcd3-fce91a2be254", "AQAAAAIAAYagAAAAEKFWJa106N0974cRdnnyYTnLEay8URTk238RAJLcaVQJlflhPHKvRwMOJNEiQz6R3A==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "specialist-user-id-constant",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "02c7179a-2121-445c-893d-6e7920aa8e9b", "AQAAAAIAAYagAAAAEG8Q7PR91jKw6hAERvVBuG2dj2luDwepG5zCdpweBMgaGS7wGRuKAQI28PCaw0o72A==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Events",
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
                values: new object[] { "87b63975-e48a-4247-9322-21db32c044cf", "AQAAAAIAAYagAAAAEJhs4B7qXAIjoviqm7ddlM1HKLWLUQNOu7OQQWaFXX+x/PsnWH/Mh5pl9xtxHTkbBQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "specialist-user-id-constant",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "34ebfc97-e6eb-4ce3-8571-8e354b210a1b", "AQAAAAIAAYagAAAAEJpINYoSC2vNcyy5m+hiGm5NdnAJBqX5QvAvMHFJ0Sp2Fl6I6/V39DeBj3zk/9B4nw==" });
        }
    }
}
