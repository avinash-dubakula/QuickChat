using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickChat.DataAccessLayer.Migrations
{
    public partial class EmptyForSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "506a60bb-79f3-45a7-a022-53ec1e62061c", "2", "User", "User" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fab4fac1-c546-41de-aebc-a14da6895711", "1", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ff8f4a2b-67d6-434e-b6cb-8cfac026e4c5", "3", "SuperUser", "HR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "506a60bb-79f3-45a7-a022-53ec1e62061c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fab4fac1-c546-41de-aebc-a14da6895711");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ff8f4a2b-67d6-434e-b6cb-8cfac026e4c5");
        }
    }
}
