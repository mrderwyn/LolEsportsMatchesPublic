using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LolEsportsMatches.DataAccess.EntityFrameworkCore.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Leagues",
                columns: table => new
                {
                    league_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    last_stored_match_id = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leagues", x => x.league_id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    team_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    region = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.team_id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    game_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    league_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    game_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    team_blue_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    team_red_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    team_blue_kills = table.Column<short>(name: "team_ blue_kills", type: "smallint", nullable: false),
                    team_red_kills = table.Column<short>(type: "smallint", nullable: false),
                    team_red_champion_1 = table.Column<short>(type: "smallint", nullable: false),
                    team_red_champion_2 = table.Column<short>(type: "smallint", nullable: false),
                    team_red_champion_3 = table.Column<short>(type: "smallint", nullable: false),
                    team_red_champion_4 = table.Column<short>(type: "smallint", nullable: false),
                    team_red_champion_5 = table.Column<short>(type: "smallint", nullable: false),
                    team_blue_champion_1 = table.Column<short>(type: "smallint", nullable: false),
                    team_blue_champion_2 = table.Column<short>(type: "smallint", nullable: false),
                    team_blue_champion_3 = table.Column<short>(type: "smallint", nullable: false),
                    team_blue_champion_4 = table.Column<short>(type: "smallint", nullable: false),
                    team_blue_champion_5 = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.game_id);
                    table.ForeignKey(
                        name: "FK_Games_Leagues_league_id",
                        column: x => x.league_id,
                        principalTable: "Leagues",
                        principalColumn: "league_id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Games_Teams_team_blue_id",
                        column: x => x.team_blue_id,
                        principalTable: "Teams",
                        principalColumn: "team_id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Games_Teams_team_red_id",
                        column: x => x.team_red_id,
                        principalTable: "Teams",
                        principalColumn: "team_id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_game_date",
                table: "Games",
                column: "game_date");

            migrationBuilder.CreateIndex(
                name: "IX_Games_league_id",
                table: "Games",
                column: "league_id");

            migrationBuilder.CreateIndex(
                name: "IX_Games_team_blue_id",
                table: "Games",
                column: "team_blue_id");

            migrationBuilder.CreateIndex(
                name: "IX_Games_team_red_id",
                table: "Games",
                column: "team_red_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Leagues");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
