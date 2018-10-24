using Microsoft.EntityFrameworkCore.Migrations;

namespace EirinDuran.WebApi.Migrations
{
    public partial class migrationssssssssssssssssssss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EncounterTeam_Teams_TeamName_SportName",
                table: "EncounterTeam");

            migrationBuilder.AddColumn<string>(
                name: "SportNameFk",
                table: "EncounterTeam",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeamNameFk",
                table: "EncounterTeam",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EncounterTeam_TeamNameFk_SportNameFk",
                table: "EncounterTeam",
                columns: new[] { "TeamNameFk", "SportNameFk" });

            migrationBuilder.AddForeignKey(
                name: "FK_EncounterTeam_Teams_TeamNameFk_SportNameFk",
                table: "EncounterTeam",
                columns: new[] { "TeamNameFk", "SportNameFk" },
                principalTable: "Teams",
                principalColumns: new[] { "Name", "SportName" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EncounterTeam_Teams_TeamNameFk_SportNameFk",
                table: "EncounterTeam");

            migrationBuilder.DropIndex(
                name: "IX_EncounterTeam_TeamNameFk_SportNameFk",
                table: "EncounterTeam");

            migrationBuilder.DropColumn(
                name: "SportNameFk",
                table: "EncounterTeam");

            migrationBuilder.DropColumn(
                name: "TeamNameFk",
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
