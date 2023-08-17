using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskSeven_GamePlatform.Server.Migrations
{
    /// <inheritdoc />
    public partial class _winner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WinnerId",
                table: "GameStates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameStates_WinnerId",
                table: "GameStates",
                column: "WinnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameStates_Players_WinnerId",
                table: "GameStates",
                column: "WinnerId",
                principalTable: "Players",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameStates_Players_WinnerId",
                table: "GameStates");

            migrationBuilder.DropIndex(
                name: "IX_GameStates_WinnerId",
                table: "GameStates");

            migrationBuilder.DropColumn(
                name: "WinnerId",
                table: "GameStates");
        }
    }
}
