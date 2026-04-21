using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PropertyPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFeatureConfigAndListingType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ListingType",
                table: "PropertyListings",
                type: "text",
                nullable: false,
                defaultValue: "sale");

            migrationBuilder.CreateTable(
                name: "FeatureConfigs",
                columns: table => new
                {
                    FeatureConfigId = table.Column<Guid>(type: "uuid", nullable: false),
                    FeatureKey = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureConfigs", x => x.FeatureConfigId);
                });

            migrationBuilder.InsertData(
                table: "FeatureConfigs",
                columns: new[] { "FeatureConfigId", "Category", "Description", "DisplayName", "FeatureKey", "IsEnabled", "SortOrder", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000101"), "ListingType", "Allow agents to publish resale or subsale property listings.", "Sale", "sale", true, 1, new DateTime(2026, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000102"), "ListingType", "Allow agents to publish rental property listings.", "Rent", "rent", true, 2, new DateTime(2026, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000103"), "ListingType", "Allow agents to publish new launch and project listings.", "New Project", "new-project", true, 3, new DateTime(2026, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PropertyListings_ListingType",
                table: "PropertyListings",
                column: "ListingType");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureConfigs_FeatureKey",
                table: "FeatureConfigs",
                column: "FeatureKey",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeatureConfigs");

            migrationBuilder.DropIndex(
                name: "IX_PropertyListings_ListingType",
                table: "PropertyListings");

            migrationBuilder.DropColumn(
                name: "ListingType",
                table: "PropertyListings");
        }
    }
}
