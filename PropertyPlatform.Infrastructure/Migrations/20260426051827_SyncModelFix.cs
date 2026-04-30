using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PropertyPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SyncModelFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Tenants_TenantId1",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_TenantId1",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_FeatureConfigs_FeatureKey",
                table: "FeatureConfigs");

            migrationBuilder.DeleteData(
                table: "FeatureConfigs",
                keyColumn: "FeatureConfigId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000101"));

            migrationBuilder.DeleteData(
                table: "FeatureConfigs",
                keyColumn: "FeatureConfigId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000102"));

            migrationBuilder.DeleteData(
                table: "FeatureConfigs",
                keyColumn: "FeatureConfigId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000103"));

            migrationBuilder.DeleteData(
                table: "Missions",
                keyColumn: "MissionId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Missions",
                keyColumn: "MissionId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"));

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "TenantId1",
                table: "RefreshTokens");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Tenants",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Tenants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Tenants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "Tenants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPhone",
                table: "Tenants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Tenants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Tenants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Tenants",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "Tenants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Tenants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Tenants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Tenants",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "ContactPhone",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Tenants");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Tenants",
                newName: "PasswordHash");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Tenants",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId1",
                table: "RefreshTokens",
                type: "uuid",
                nullable: true);

            migrationBuilder.InsertData(
                table: "FeatureConfigs",
                columns: new[] { "FeatureConfigId", "Category", "Description", "DisplayName", "FeatureKey", "IsEnabled", "SortOrder", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000101"), "ListingType", "Allow agents to publish resale or subsale property listings.", "Sale", "sale", true, 1, new DateTime(2026, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000102"), "ListingType", "Allow agents to publish rental property listings.", "Rent", "rent", true, 2, new DateTime(2026, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000103"), "ListingType", "Allow agents to publish new launch and project listings.", "New Project", "new-project", true, 3, new DateTime(2026, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Missions",
                columns: new[] { "MissionId", "Code", "CreditReward", "Description", "RequirementCount", "Title", "XPReward" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000003"), "FIRST_LISTING", 10, "Upload your very first property listing.", 1, "First Listing", 100 },
                    { new Guid("00000000-0000-0000-0000-000000000004"), "UPLOAD_5_LISTINGS", 50, "Upload 5 properties to the platform.", 5, "Listing Spree", 300 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_TenantId1",
                table: "RefreshTokens",
                column: "TenantId1");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureConfigs_FeatureKey",
                table: "FeatureConfigs",
                column: "FeatureKey",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Tenants_TenantId1",
                table: "RefreshTokens",
                column: "TenantId1",
                principalTable: "Tenants",
                principalColumn: "TenantId");
        }
    }
}
