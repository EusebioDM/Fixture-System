using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SilverFixture.WebApi.Migrations
{
    public partial class Teampositions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sports",
                columns: table => new
                {
                    SportName = table.Column<string>(nullable: false),
                    EncounterPlayerCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sports", x => x.SportName);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserName = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Mail = table.Column<string>(nullable: true),
                    Role = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserName);
                });

            migrationBuilder.CreateTable(
                name: "Encounters",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    SportName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Encounters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Encounters_Sports_SportName",
                        column: x => x.SportName,
                        principalTable: "Sports",
                        principalColumn: "SportName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    SportName = table.Column<string>(nullable: false),
                    Logo = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => new { x.Name, x.SportName });
                    table.ForeignKey(
                        name: "FK_Teams_Sports_SportName",
                        column: x => x.SportName,
                        principalTable: "Sports",
                        principalColumn: "SportName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    UserName = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    Id = table.Column<Guid>(nullable: false),
                    EncounterEntityId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Encounters_EncounterEntityId",
                        column: x => x.EncounterEntityId,
                        principalTable: "Encounters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserName",
                        column: x => x.UserName,
                        principalTable: "Users",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EncounterTeam",
                columns: table => new
                {
                    EncounterId = table.Column<Guid>(nullable: false),
                    TeamName = table.Column<string>(nullable: false),
                    SportName = table.Column<string>(nullable: false),
                    TeamNameFk = table.Column<string>(nullable: true),
                    SportNameFk = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EncounterTeam", x => new { x.TeamName, x.SportName, x.EncounterId });
                    table.ForeignKey(
                        name: "FK_EncounterTeam_Encounters_EncounterId",
                        column: x => x.EncounterId,
                        principalTable: "Encounters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EncounterTeam_Teams_TeamNameFk_SportNameFk",
                        columns: x => new { x.TeamNameFk, x.SportNameFk },
                        principalTable: "Teams",
                        principalColumns: new[] { "Name", "SportName" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamResult",
                columns: table => new
                {
                    TeamName = table.Column<string>(nullable: true),
                    TeamSportName = table.Column<string>(nullable: true),
                    EncounterId = table.Column<Guid>(nullable: false),
                    TeamId = table.Column<string>(nullable: false),
                    Position = table.Column<int>(nullable: false),
                    EncounterEntityId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamResult", x => new { x.TeamId, x.EncounterId });
                    table.ForeignKey(
                        name: "FK_TeamResult_Encounters_EncounterEntityId",
                        column: x => x.EncounterEntityId,
                        principalTable: "Encounters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamResult_Teams_TeamName_TeamSportName",
                        columns: x => new { x.TeamName, x.TeamSportName },
                        principalTable: "Teams",
                        principalColumns: new[] { "Name", "SportName" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamUsers",
                columns: table => new
                {
                    TeamName = table.Column<string>(nullable: false),
                    SportName = table.Column<string>(nullable: false),
                    TeamName1 = table.Column<string>(nullable: true),
                    TeamSportName = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamUsers", x => new { x.TeamName, x.SportName, x.UserName });
                    table.ForeignKey(
                        name: "FK_TeamUsers_Users_UserName",
                        column: x => x.UserName,
                        principalTable: "Users",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamUsers_Teams_TeamName1_TeamSportName",
                        columns: x => new { x.TeamName1, x.TeamSportName },
                        principalTable: "Teams",
                        principalColumns: new[] { "Name", "SportName" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_EncounterEntityId",
                table: "Comments",
                column: "EncounterEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserName",
                table: "Comments",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_SportName",
                table: "Encounters",
                column: "SportName");

            migrationBuilder.CreateIndex(
                name: "IX_EncounterTeam_EncounterId",
                table: "EncounterTeam",
                column: "EncounterId");

            migrationBuilder.CreateIndex(
                name: "IX_EncounterTeam_TeamNameFk_SportNameFk",
                table: "EncounterTeam",
                columns: new[] { "TeamNameFk", "SportNameFk" });

            migrationBuilder.CreateIndex(
                name: "IX_TeamResult_EncounterEntityId",
                table: "TeamResult",
                column: "EncounterEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamResult_TeamName_TeamSportName",
                table: "TeamResult",
                columns: new[] { "TeamName", "TeamSportName" });

            migrationBuilder.CreateIndex(
                name: "IX_Teams_SportName",
                table: "Teams",
                column: "SportName");

            migrationBuilder.CreateIndex(
                name: "IX_TeamUsers_UserName",
                table: "TeamUsers",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "IX_TeamUsers_TeamName1_TeamSportName",
                table: "TeamUsers",
                columns: new[] { "TeamName1", "TeamSportName" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "EncounterTeam");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "TeamResult");

            migrationBuilder.DropTable(
                name: "TeamUsers");

            migrationBuilder.DropTable(
                name: "Encounters");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Sports");
        }
    }
}
