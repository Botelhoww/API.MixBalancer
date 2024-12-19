using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixBalancer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMatchIdCS2ToMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ManagedByUserId",
                table: "Matches",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "MatchIdCS2",
                table: "Matches",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManagedByUserId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "MatchIdCS2",
                table: "Matches");
        }
    }
}
