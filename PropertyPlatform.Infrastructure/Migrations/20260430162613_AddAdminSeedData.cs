using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "TenantId", "Address", "City", "ContactEmail", "ContactPhone", "Country", "CreatedAt", "Description", "IsActive", "LogoUrl", "Name", "PostalCode", "State", "UpdatedAt" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000100"), null, null, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, null, "Platform Administration", null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "AgentProfiles",
                columns: new[] { "AgentId", "Bio", "CompanyLogoUrl", "Credits", "Email", "ExperiencePoints", "Level", "Name", "OfficeAddress", "PasswordHash", "Phone", "ProfilePhotoUrl", "REN_ID", "Slug", "TenantId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000101"), "System Administrator", "https://via.placeholder.com/150", 0, "admin", 0, 1, "Platform Admin", "System HQ", "$2a$11$pxNvtTsemAwY0LSFV3JE6.k2WEG1OzVBkbMHIOs3rsQmUDrTV2.r.", "000-000-0000", "https://via.placeholder.com/150", "ADMIN-001", "admin", new Guid("00000000-0000-0000-0000-000000000100") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AgentProfiles",
                keyColumn: "AgentId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000101"));

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "TenantId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000100"));
        }
    }
}
