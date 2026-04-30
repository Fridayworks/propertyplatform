using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixFeatureConfigKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "FeatureConfigs",
                keyColumn: "FeatureConfigId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000201"),
                column: "FeatureKey",
                value: "sale");

            migrationBuilder.UpdateData(
                table: "FeatureConfigs",
                keyColumn: "FeatureConfigId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000202"),
                column: "FeatureKey",
                value: "rent");

            migrationBuilder.UpdateData(
                table: "FeatureConfigs",
                keyColumn: "FeatureConfigId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000203"),
                column: "FeatureKey",
                value: "new-project");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "FeatureConfigs",
                keyColumn: "FeatureConfigId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000201"),
                column: "FeatureKey",
                value: "ENABLE_SALE");

            migrationBuilder.UpdateData(
                table: "FeatureConfigs",
                keyColumn: "FeatureConfigId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000202"),
                column: "FeatureKey",
                value: "ENABLE_RENT");

            migrationBuilder.UpdateData(
                table: "FeatureConfigs",
                keyColumn: "FeatureConfigId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000203"),
                column: "FeatureKey",
                value: "ENABLE_NEW_PROJECT");
        }
    }
}
