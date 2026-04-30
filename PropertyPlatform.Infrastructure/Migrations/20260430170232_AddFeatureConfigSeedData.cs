using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PropertyPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFeatureConfigSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FeatureConfigs",
                columns: new[] { "FeatureConfigId", "Category", "Description", "DisplayName", "FeatureKey", "IsEnabled", "SortOrder", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000201"), "ListingType", "Allow agents to create listings for property sales.", "Property Sale", "ENABLE_SALE", true, 1, new DateTime(2026, 4, 30, 17, 2, 31, 710, DateTimeKind.Utc).AddTicks(8750) },
                    { new Guid("00000000-0000-0000-0000-000000000202"), "ListingType", "Allow agents to create listings for residential or commercial rentals.", "Property Rental", "ENABLE_RENT", true, 2, new DateTime(2026, 4, 30, 17, 2, 31, 711, DateTimeKind.Utc).AddTicks(797) },
                    { new Guid("00000000-0000-0000-0000-000000000203"), "ListingType", "Allow agents to create listings for new developer projects.", "New Projects", "ENABLE_NEW_PROJECT", true, 3, new DateTime(2026, 4, 30, 17, 2, 31, 711, DateTimeKind.Utc).AddTicks(802) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FeatureConfigs",
                keyColumn: "FeatureConfigId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000201"));

            migrationBuilder.DeleteData(
                table: "FeatureConfigs",
                keyColumn: "FeatureConfigId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000202"));

            migrationBuilder.DeleteData(
                table: "FeatureConfigs",
                keyColumn: "FeatureConfigId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000203"));
        }
    }
}
