using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickChat.DataAccessLayer.Migrations
{
    public partial class UpdatingColumTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityUser");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "506a60bb-79f3-45a7-a022-53ec1e62061c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ff8f4a2b-67d6-434e-b6cb-8cfac026e4c5");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "fab4fac1-c546-41de-aebc-a14da6895711", "b74ddd14-6340-4840-95c2-db12554843e5" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fab4fac1-c546-41de-aebc-a14da6895711");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "UserGroups",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserGroupParticipants",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CreatorId",
                table: "UserGroups",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserGroupParticipants",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "IdentityUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUser", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "506a60bb-79f3-45a7-a022-53ec1e62061c", "2", "User", "User" },
                    { "fab4fac1-c546-41de-aebc-a14da6895711", "1", "Admin", "Admin" },
                    { "ff8f4a2b-67d6-434e-b6cb-8cfac026e4c5", "3", "HR", "HR" }
                });

            migrationBuilder.InsertData(
                table: "IdentityUser",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "b74ddd14-6340-4840-95c2-db12554843e5", 0, "15a4aaea-2377-409a-b540-4553d636d47a", "admin@gmail.com", false, false, null, null, null, "AQAAAAEAACcQAAAAEFpnc8KLo11nFxatkZjrkOasffpGpSx3zrXQ+lGBzIQTrDR/c5TsfNvqPsg4yijcDg==", "1234567890", false, "99b6cef6-8c72-4d2c-80f0-c6c89cd0e96a", false, "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "fab4fac1-c546-41de-aebc-a14da6895711", "b74ddd14-6340-4840-95c2-db12554843e5" });
        }
    }
}
