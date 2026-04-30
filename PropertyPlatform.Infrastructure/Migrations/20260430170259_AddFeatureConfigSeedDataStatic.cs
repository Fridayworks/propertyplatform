using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFeatureConfigSeedDataStatic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "FeatureConfigs",
                keyColumn: "FeatureConfigId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000201"),
                column: "UpdatedAt",
                value: new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "FeatureConfigs",
                keyColumn: "FeatureConfigId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000202"),
                column: "UpdatedAt",
                value: new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "FeatureConfigs",
                keyColumn: "FeatureConfigId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000203"),
                column: "UpdatedAt",
                value: new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "FeatureConfigs",
                keyColumn: "FeatureConfigId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000201"),
                column: "UpdatedAt",
                value: new DateTime(2026, 4, 30, 17, 2, 31, 710, DateTimeKind.Utc).AddTicks(8750));

            migrationBuilder.UpdateData(
                table: "FeatureConfigs",
                keyColumn: "FeatureConfigId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000202"),
                column: "UpdatedAt",
                value: new DateTime(2026, 4, 30, 17, 2, 31, 711, DateTimeKind.Utc).AddTicks(797));

            migrationBuilder.UpdateData(
                table: "FeatureConfigs",
                keyColumn: "FeatureConfigId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000203"),
                column: "UpdatedAt",
                value: new DateTime(2026, 4, 30, 17, 2, 31, 711, DateTimeKind.Utc).AddTicks(802));
        }
    }
}
