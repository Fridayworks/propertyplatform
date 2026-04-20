using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PropertyPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SyncModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId1",
                table: "RefreshTokens",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "AgentProfiles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ExperiencePoints",
                table: "AgentProfiles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "AgentProfiles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "AgentProfiles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AgentReviews",
                columns: table => new
                {
                    ReviewId = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReviewerName = table.Column<string>(type: "text", nullable: false),
                    ReviewerEmail = table.Column<string>(type: "text", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentReviews", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_AgentReviews_Tenants_AgentTenantId",
                        column: x => x.AgentTenantId,
                        principalTable: "Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Badges",
                columns: table => new
                {
                    BadgeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IconUrl = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Badges", x => x.BadgeId);
                });

            migrationBuilder.CreateTable(
                name: "CreditTransactions",
                columns: table => new
                {
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditTransactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_CreditTransactions_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Missions",
                columns: table => new
                {
                    MissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    RequirementCount = table.Column<int>(type: "integer", nullable: false),
                    CreditReward = table.Column<int>(type: "integer", nullable: false),
                    XPReward = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Missions", x => x.MissionId);
                });

            migrationBuilder.CreateTable(
                name: "AgentBadges",
                columns: table => new
                {
                    AgentBadgeId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    BadgeId = table.Column<Guid>(type: "uuid", nullable: false),
                    AwardedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentBadges", x => x.AgentBadgeId);
                    table.ForeignKey(
                        name: "FK_AgentBadges_Badges_BadgeId",
                        column: x => x.BadgeId,
                        principalTable: "Badges",
                        principalColumn: "BadgeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgentBadges_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AgentMissions",
                columns: table => new
                {
                    AgentMissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    MissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrentProgress = table.Column<int>(type: "integer", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentMissions", x => x.AgentMissionId);
                    table.ForeignKey(
                        name: "FK_AgentMissions_Missions_MissionId",
                        column: x => x.MissionId,
                        principalTable: "Missions",
                        principalColumn: "MissionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgentMissions_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Badges",
                columns: new[] { "BadgeId", "Code", "Description", "IconUrl", "Name" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), "RISING_STAR", "Joined the platform and completed initial setup.", "✨", "Rising Star" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), "ELITE_LISTER", "Uploaded 10 high-quality listings.", "🏆", "Elite Lister" }
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
                name: "IX_AgentProfiles_Slug",
                table: "AgentProfiles",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgentBadges_BadgeId",
                table: "AgentBadges",
                column: "BadgeId");

            migrationBuilder.CreateIndex(
                name: "IX_AgentBadges_TenantId",
                table: "AgentBadges",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AgentMissions_MissionId",
                table: "AgentMissions",
                column: "MissionId");

            migrationBuilder.CreateIndex(
                name: "IX_AgentMissions_TenantId",
                table: "AgentMissions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AgentReviews_AgentTenantId",
                table: "AgentReviews",
                column: "AgentTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditTransactions_TenantId",
                table: "CreditTransactions",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Tenants_TenantId1",
                table: "RefreshTokens",
                column: "TenantId1",
                principalTable: "Tenants",
                principalColumn: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Tenants_TenantId1",
                table: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "AgentBadges");

            migrationBuilder.DropTable(
                name: "AgentMissions");

            migrationBuilder.DropTable(
                name: "AgentReviews");

            migrationBuilder.DropTable(
                name: "CreditTransactions");

            migrationBuilder.DropTable(
                name: "Badges");

            migrationBuilder.DropTable(
                name: "Missions");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_TenantId1",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_AgentProfiles_Slug",
                table: "AgentProfiles");

            migrationBuilder.DropColumn(
                name: "TenantId1",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "AgentProfiles");

            migrationBuilder.DropColumn(
                name: "ExperiencePoints",
                table: "AgentProfiles");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "AgentProfiles");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "AgentProfiles");
        }
    }
}
