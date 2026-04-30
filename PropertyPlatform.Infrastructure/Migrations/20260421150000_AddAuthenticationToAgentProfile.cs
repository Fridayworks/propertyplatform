using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthenticationToAgentProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "AgentProfiles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "AgentProfiles",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "AgentProfiles");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "AgentProfiles");
        }
    }
}
