using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskSeven_GamePlatform.Server.Migrations
{
    /// <inheritdoc />
    public partial class _init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldSize = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPlaying = table.Column<bool>(type: "bit", nullable: false),
                    WaitingForMove = table.Column<bool>(type: "bit", nullable: false),
                    LookingForOpponent = table.Column<bool>(type: "bit", nullable: false),
                    GameStarted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConnectionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentGameTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_GameTypes_CurrentGameTypeId",
                        column: x => x.CurrentGameTypeId,
                        principalTable: "GameTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GameStates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsGameOver = table.Column<bool>(type: "bit", nullable: false),
                    IsDraw = table.Column<bool>(type: "bit", nullable: false),
                    Player1Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Player2Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Field = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovesLeft = table.Column<int>(type: "int", nullable: false),
                    SecondsPerMove = table.Column<int>(type: "int", nullable: false),
                    LastMove = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GameTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameStates_GameTypes_GameTypeId",
                        column: x => x.GameTypeId,
                        principalTable: "GameTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameStates_Players_Player1Id",
                        column: x => x.Player1Id,
                        principalTable: "Players",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GameStates_Players_Player2Id",
                        column: x => x.Player2Id,
                        principalTable: "Players",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "GameTypes",
                columns: new[] { "Id", "FieldSize", "Name" },
                values: new object[] { new Guid("706c2e99-6f6c-4472-81a5-43c56e11637c"), 9, "" });

            migrationBuilder.CreateIndex(
                name: "IX_GameStates_GameTypeId",
                table: "GameStates",
                column: "GameTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GameStates_Player1Id",
                table: "GameStates",
                column: "Player1Id");

            migrationBuilder.CreateIndex(
                name: "IX_GameStates_Player2Id",
                table: "GameStates",
                column: "Player2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Players_CurrentGameTypeId",
                table: "Players",
                column: "CurrentGameTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameStates");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "GameTypes");
        }
    }
}
