using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskSeven_GamePlatform.Server.Migrations
{
    /// <inheritdoc />
    public partial class _lazyLoading : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Players_OpponentId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_OpponentId",
                table: "Players");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "GameTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "GameTypes",
                columns: new[] { "Id", "FieldSize", "Name" },
                values: new object[] { new Guid("706c2e99-6f6c-4472-81a5-43c56e11637c"), 9, "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: new Guid("706c2e99-6f6c-4472-81a5-43c56e11637c"));

            migrationBuilder.DropColumn(
                name: "Name",
                table: "GameTypes");

            migrationBuilder.CreateIndex(
                name: "IX_Players_OpponentId",
                table: "Players",
                column: "OpponentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Players_OpponentId",
                table: "Players",
                column: "OpponentId",
                principalTable: "Players",
                principalColumn: "Id");
        }
    }
}
