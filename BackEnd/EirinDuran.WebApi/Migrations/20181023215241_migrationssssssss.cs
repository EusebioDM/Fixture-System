using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EirinDuran.WebApi.Migrations
{
    public partial class migrationssssssss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Encounters_Teams_AwayTeamName_AwayTeamSportName",
                table: "Encounters");

            migrationBuilder.DropForeignKey(
                name: "FK_Encounters_Teams_HomeTeamName_HomeTeamSportName",
                table: "Encounters");

            migrationBuilder.DropIndex(
                name: "IX_Encounters_AwayTeamName_AwayTeamSportName",
                table: "Encounters");

            migrationBuilder.DropIndex(
                name: "IX_Encounters_HomeTeamName_HomeTeamSportName",
                table: "Encounters");

            migrationBuilder.DropColumn(
                name: "AwayTeamName",
                table: "Encounters");

            migrationBuilder.DropColumn(
                name: "AwayTeamSportName",
                table: "Encounters");

            migrationBuilder.DropColumn(
                name: "HomeTeamName",
                table: "Encounters");

            migrationBuilder.DropColumn(
                name: "HomeTeamSportName",
                table: "Encounters");

            migrationBuilder.CreateTable(
                name: "EncounterTeam",
                columns: table => new
                {
                    EncounterId = table.Column<Guid>(nullable: false),
                    TeamName = table.Column<string>(nullable: false),
                    SportName = table.Column<string>(nullable: false)
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
                        name: "FK_EncounterTeam_Teams_TeamName_SportName",
                        columns: x => new { x.TeamName, x.SportName },
                        principalTable: "Teams",
                        principalColumns: new[] { "Name", "SportName" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EncounterTeam_EncounterId",
                table: "EncounterTeam",
                column: "EncounterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EncounterTeam");

            migrationBuilder.AddColumn<string>(
                name: "AwayTeamName",
                table: "Encounters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AwayTeamSportName",
                table: "Encounters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeTeamName",
                table: "Encounters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeTeamSportName",
                table: "Encounters",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_AwayTeamName_AwayTeamSportName",
                table: "Encounters",
                columns: new[] { "AwayTeamName", "AwayTeamSportName" });

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_HomeTeamName_HomeTeamSportName",
                table: "Encounters",
                columns: new[] { "HomeTeamName", "HomeTeamSportName" });

            migrationBuilder.AddForeignKey(
                name: "FK_Encounters_Teams_AwayTeamName_AwayTeamSportName",
                table: "Encounters",
                columns: new[] { "AwayTeamName", "AwayTeamSportName" },
                principalTable: "Teams",
                principalColumns: new[] { "Name", "SportName" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Encounters_Teams_HomeTeamName_HomeTeamSportName",
                table: "Encounters",
                columns: new[] { "HomeTeamName", "HomeTeamSportName" },
                principalTable: "Teams",
                principalColumns: new[] { "Name", "SportName" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
