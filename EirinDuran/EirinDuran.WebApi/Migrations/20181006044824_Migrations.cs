using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EirinDuran.WebApi.Migrations
{
    public partial class Migrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sports",
                columns: table => new
                {
                    SportName = table.Column<string>(nullable: false)
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
                name: "Encounters",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    SportName = table.Column<string>(nullable: true),
                    HomeTeamName = table.Column<string>(nullable: true),
                    HomeTeamSportName = table.Column<string>(nullable: true),
                    AwayTeamName = table.Column<string>(nullable: true),
                    AwayTeamSportName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Encounters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Encounters_Sports_SportName",
                        column: x => x.SportName,
                        principalTable: "Sports",
                        principalColumn: "SportName",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Encounters_Teams_AwayTeamName_AwayTeamSportName",
                        columns: x => new { x.AwayTeamName, x.AwayTeamSportName },
                        principalTable: "Teams",
                        principalColumns: new[] { "Name", "SportName" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Encounters_Teams_HomeTeamName_HomeTeamSportName",
                        columns: x => new { x.HomeTeamName, x.HomeTeamSportName },
                        principalTable: "Teams",
                        principalColumns: new[] { "Name", "SportName" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamUsers",
                columns: table => new
                {
                    TeamName = table.Column<string>(nullable: false),
                    SportName = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamUsers", x => new { x.TeamName, x.UserName });
                    table.ForeignKey(
                        name: "FK_TeamUsers_Users_UserName",
                        column: x => x.UserName,
                        principalTable: "Users",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamUsers_Teams_TeamName_SportName",
                        columns: x => new { x.TeamName, x.SportName },
                        principalTable: "Teams",
                        principalColumns: new[] { "Name", "SportName" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommentEntity",
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
                    table.PrimaryKey("PK_CommentEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentEntity_Encounters_EncounterEntityId",
                        column: x => x.EncounterEntityId,
                        principalTable: "Encounters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommentEntity_Users_UserName",
                        column: x => x.UserName,
                        principalTable: "Users",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentEntity_EncounterEntityId",
                table: "CommentEntity",
                column: "EncounterEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentEntity_UserName",
                table: "CommentEntity",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_SportName",
                table: "Encounters",
                column: "SportName");

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_AwayTeamName_AwayTeamSportName",
                table: "Encounters",
                columns: new[] { "AwayTeamName", "AwayTeamSportName" });

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_HomeTeamName_HomeTeamSportName",
                table: "Encounters",
                columns: new[] { "HomeTeamName", "HomeTeamSportName" });

            migrationBuilder.CreateIndex(
                name: "IX_Teams_SportName",
                table: "Teams",
                column: "SportName");

            migrationBuilder.CreateIndex(
                name: "IX_TeamUsers_UserName",
                table: "TeamUsers",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "IX_TeamUsers_TeamName_SportName",
                table: "TeamUsers",
                columns: new[] { "TeamName", "SportName" },
                unique: true,
                filter: "[SportName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentEntity");

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
