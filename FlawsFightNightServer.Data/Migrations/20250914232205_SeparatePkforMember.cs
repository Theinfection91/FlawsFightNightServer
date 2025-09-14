using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlawsFightNightServer.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeparatePkforMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Members",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Members");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Members",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()"); // or Guid.NewGuid() for EF default

            migrationBuilder.AddColumn<string>(
                name: "DiscordId",
                table: "Members",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Members",
                table: "Members",
                column: "DiscordId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_DiscordId",
                table: "Members",
                column: "DiscordId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Members",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_DiscordId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "DiscordId",
                table: "Members");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Members",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Members",
                table: "Members",
                column: "Id");
        }
    }
}
