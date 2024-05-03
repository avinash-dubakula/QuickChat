using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickChat.DataAccessLayer.Migrations
{
    public partial class AddingTablesAndUpdatingIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePhotoUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "IdentityUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserFriendRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderUserId = table.Column<int>(type: "int", nullable: false),
                    RecieverUserId = table.Column<int>(type: "int", nullable: false),
                    FriendRequestStatus = table.Column<int>(type: "int", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFriendRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatorId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfilePhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecieverUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageStatus = table.Column<int>(type: "int", nullable: false),
                    MessageSentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MessageDeliveredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MessageSeenAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserGroupParticipants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserGroupId = table.Column<int>(type: "int", nullable: false),
                    GroupRole = table.Column<int>(type: "int", nullable: false),
                    AddedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedBy = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroupParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGroupParticipants_UserGroups_UserGroupId",
                        column: x => x.UserGroupId,
                        principalTable: "UserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "IdentityUser",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "b74ddd14-6340-4840-95c2-db12554843e5", 0, "15a4aaea-2377-409a-b540-4553d636d47a", "admin@gmail.com", false, false, null, null, null, "AQAAAAEAACcQAAAAEFpnc8KLo11nFxatkZjrkOasffpGpSx3zrXQ+lGBzIQTrDR/c5TsfNvqPsg4yijcDg==", "1234567890", false, "99b6cef6-8c72-4d2c-80f0-c6c89cd0e96a", false, "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupParticipants_UserGroupId",
                table: "UserGroupParticipants",
                column: "UserGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityUser");

            migrationBuilder.DropTable(
                name: "UserFriendRequests");

            migrationBuilder.DropTable(
                name: "UserGroupParticipants");

            migrationBuilder.DropTable(
                name: "UserMessages");

            migrationBuilder.DropTable(
                name: "UserGroups");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfilePhotoUrl",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "b74ddd14-6340-4840-95c2-db12554843e5", 0, "b96777a0-a7d2-4e42-95ba-7d67e68ff8b4", "admin@gmail.com", false, false, null, null, null, "AQAAAAEAACcQAAAAEJDO6gXcVhcqaPdpPTD4BbxtnHYQAyzaZTWJfbPvdlRssd2U39r8f0MM4wcAsUJm4w==", "1234567890", false, "9b471359-fdb5-4c6b-aff6-1526ce4d07c2", false, "Admin" });
        }
    }
}
