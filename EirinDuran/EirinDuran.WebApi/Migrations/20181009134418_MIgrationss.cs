using Microsoft.EntityFrameworkCore.Migrations;

namespace EirinDuran.WebApi.Migrations
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public partial class Migrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamUsers_Teams_TeamName_SportName",
                table: "TeamUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamUsers_Teams_TeamName_SportName",
                table: "TeamUsers",
                columns: new[] { "TeamName", "SportName" },
                principalTable: "Teams",
                principalColumns: new[] { "Name", "SportName" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamUsers_Teams_TeamName_SportName",
                table: "TeamUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamUsers_Teams_TeamName_SportName",
                table: "TeamUsers",
                columns: new[] { "TeamName", "SportName" },
                principalTable: "Teams",
                principalColumns: new[] { "Name", "SportName" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
