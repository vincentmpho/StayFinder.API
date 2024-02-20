using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StayFinder.API.Migrations
{
    public partial class AddedDefaultRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8539015e-353f-4c35-95a0-84a5da0f2087", "1a20a9be-3829-4e4b-ac99-0120f1a6abd8", "User", "USER" },
                    { "a7169623-f380-496c-859a-5cab90e79941", "6f8eaf6b-9a33-443e-b143-680bed1e1702", "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "countries",
                columns: new[] { "Id", "Name", "ShortName" },
                values: new object[,]
                {
                    { 1, "Jamica", "JM" },
                    { 2, "South Africa", "SA" },
                    { 3, "South Africa", "SA" }
                });

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "Id", "Address", "CountryId", "Name", "Rating" },
                values: new object[] { 1, "Negril", 1, "Sandals Resort and Spa", 4.2000000000000002 });

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "Id", "Address", "CountryId", "Name", "Rating" },
                values: new object[] { 2, "Limpopo", 3, "Thaba Pitsi Nature Reserve and Spa", 5.0 });

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "Id", "Address", "CountryId", "Name", "Rating" },
                values: new object[] { 3, "Cape Town", 2, "Table Bay Hotel", 4.2000000000000002 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8539015e-353f-4c35-95a0-84a5da0f2087");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a7169623-f380-496c-859a-5cab90e79941");

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "countries",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "countries",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "countries",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
