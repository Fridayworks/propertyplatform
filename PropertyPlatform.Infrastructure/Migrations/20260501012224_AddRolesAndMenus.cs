using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PropertyPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRolesAndMenus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AdminRoleId",
                table: "AgentProfiles",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AgentProfiles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "AdminRoles",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Permissions = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminRoles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "DynamicMenus",
                columns: table => new
                {
                    MenuId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Location = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicMenus", x => x.MenuId);
                });

            migrationBuilder.InsertData(
                table: "AdminRoles",
                columns: new[] { "RoleId", "CreatedAt", "Description", "Name", "Permissions", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000401"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Full access to all modules and configurations.", "Super Admin", "CMS.Manage,Users.Manage,Roles.Manage,Menus.Manage,Config.Manage", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000402"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Access to manage CMS articles and news only.", "Content Editor", "CMS.Manage", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.UpdateData(
                table: "AgentProfiles",
                keyColumn: "AgentId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000101"),
                columns: new[] { "AdminRoleId", "UpdatedAt" },
                values: new object[] { null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "DynamicMenus",
                columns: new[] { "MenuId", "CreatedAt", "IsActive", "Location", "SortOrder", "Title", "UpdatedAt", "Url" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000501"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Main", 1, "Home", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "/" },
                    { new Guid("00000000-0000-0000-0000-000000000502"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Main", 2, "Find a Home", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "/Search" },
                    { new Guid("00000000-0000-0000-0000-000000000503"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Main", 3, "News", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "/News" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgentProfiles_AdminRoleId",
                table: "AgentProfiles",
                column: "AdminRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AgentProfiles_AdminRoles_AdminRoleId",
                table: "AgentProfiles",
                column: "AdminRoleId",
                principalTable: "AdminRoles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgentProfiles_AdminRoles_AdminRoleId",
                table: "AgentProfiles");

            migrationBuilder.DropTable(
                name: "AdminRoles");

            migrationBuilder.DropTable(
                name: "DynamicMenus");

            migrationBuilder.DropIndex(
                name: "IX_AgentProfiles_AdminRoleId",
                table: "AgentProfiles");

            migrationBuilder.DropColumn(
                name: "AdminRoleId",
                table: "AgentProfiles");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AgentProfiles");
        }
    }
}
