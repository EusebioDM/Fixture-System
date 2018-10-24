using Microsoft.EntityFrameworkCore.Migrations;

namespace EirinDuran.WebApi.Migrations
{
    public partial class migrationssssssssss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EncounterTeam_Teams_TeamName_SportName",
                table: "EncounterTeam");

            migrationBuilder.AddForeignKey(
                name: "FK_EncounterTeam_Teams_TeamName_SportName",
                table: "EncounterTeam",
                columns: new[] { "TeamName", "SportName" },
                principalTable: "Teams",
                principalColumns: new[] { "Name", "SportName" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EncounterTeam_Teams_TeamName_SportName",
                table: "EncounterTeam");

            migrationBuilder.AddForeignKey(
                name: "FK_EncounterTeam_Teams_TeamName_SportName",
                table: "EncounterTeam",
                columns: new[] { "TeamName", "SportName" },
                principalTable: "Teams",
                principalColumns: new[] { "Name", "SportName" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
