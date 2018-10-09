using Microsoft.EntityFrameworkCore.Migrations;

namespace EirinDuran.WebApi.Migrations
{
    public partial class Migrationss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Encounters_Sports_SportName",
                table: "Encounters");

            migrationBuilder.DropForeignKey(
                name: "FK_Encounters_Teams_AwayTeamName_AwayTeamSportName",
                table: "Encounters");

            migrationBuilder.DropForeignKey(
                name: "FK_Encounters_Teams_HomeTeamName_HomeTeamSportName",
                table: "Encounters");

            migrationBuilder.AddForeignKey(
                name: "FK_Encounters_Sports_SportName",
                table: "Encounters",
                column: "SportName",
                principalTable: "Sports",
                principalColumn: "SportName",
                onDelete: ReferentialAction.Cascade);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Encounters_Sports_SportName",
                table: "Encounters");

            migrationBuilder.DropForeignKey(
                name: "FK_Encounters_Teams_AwayTeamName_AwayTeamSportName",
                table: "Encounters");

            migrationBuilder.DropForeignKey(
                name: "FK_Encounters_Teams_HomeTeamName_HomeTeamSportName",
                table: "Encounters");

            migrationBuilder.AddForeignKey(
                name: "FK_Encounters_Sports_SportName",
                table: "Encounters",
                column: "SportName",
                principalTable: "Sports",
                principalColumn: "SportName",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Encounters_Teams_AwayTeamName_AwayTeamSportName",
                table: "Encounters",
                columns: new[] { "AwayTeamName", "AwayTeamSportName" },
                principalTable: "Teams",
                principalColumns: new[] { "Name", "SportName" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Encounters_Teams_HomeTeamName_HomeTeamSportName",
                table: "Encounters",
                columns: new[] { "HomeTeamName", "HomeTeamSportName" },
                principalTable: "Teams",
                principalColumns: new[] { "Name", "SportName" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
