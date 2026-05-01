using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PropertyPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCMSAndNews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    ArticleId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Excerpt = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Author = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.ArticleId);
                });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "ArticleId", "Author", "Category", "Content", "CreatedAt", "Excerpt", "PublishedAt", "Slug", "Status", "ThumbnailUrl", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000301"), "System Admin", "Company News", "<p>Welcome to the <strong>Annie Rustic Property Platform</strong>. We have completely rewritten our core engine to provide a seamless, AI-powered experience for agents and buyers alike.</p><p>Explore our new features including AI-driven listing automation, smart lead management, and premium agent microsites.</p>", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "We are excited to announce the launch of our next-generation property ecosystem.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "welcome-to-new-platform", "Published", "https://images.unsplash.com/photo-1486406146926-c627a92ad1ab?auto=format&fit=crop&q=80&w=800", "Welcome to the New Property Platform", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000302"), "Market Analytics Team", "Market Trends", "<p>The residential market in early 2026 shows a strong preference for mixed-use developments and smart home integrations.</p><p>Our data indicates a 15% increase in searches for properties with dedicated home office spaces and high-speed fiber connectivity.</p>", new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Analyzing the shift in urban living preferences and price stability in major metropolitan areas.", new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "market-trends-q1-2026", "Published", "https://images.unsplash.com/photo-1460472178825-e52506b3f90a?auto=format&fit=crop&q=80&w=800", "Market Trends: Q1 2026 Residential Outlook", new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_Category",
                table: "Articles",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_Slug",
                table: "Articles",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_Status",
                table: "Articles",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");
        }
    }
}
