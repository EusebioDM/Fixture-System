using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EirinDuran.WebApi.Migrations
{
    public partial class Migrations_Without_DB_generated_ID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sports",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sports", x => x.Name);
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
                    Logo = table.Column<byte[]>(nullable: true),
                    SportEntityName = table.Column<string>(nullable: true),
                    UserEntityUserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Teams_Sports_SportEntityName",
                        column: x => x.SportEntityName,
                        principalTable: "Sports",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Teams_Users_UserEntityUserName",
                        column: x => x.UserEntityUserName,
                        principalTable: "Users",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Encounters",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    SportName = table.Column<string>(nullable: true),
                    HomeTeamName = table.Column<string>(nullable: true),
                    AwayTeamName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Encounters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Encounters_Teams_AwayTeamName",
                        column: x => x.AwayTeamName,
                        principalTable: "Teams",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Encounters_Teams_HomeTeamName",
                        column: x => x.HomeTeamName,
                        principalTable: "Teams",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Encounters_Sports_SportName",
                        column: x => x.SportName,
                        principalTable: "Sports",
                        principalColumn: "Name",
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
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_Encounters_AwayTeamName",
                table: "Encounters",
                column: "AwayTeamName");

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_HomeTeamName",
                table: "Encounters",
                column: "HomeTeamName");

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_SportName",
                table: "Encounters",
                column: "SportName");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_SportEntityName",
                table: "Teams",
                column: "SportEntityName");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_UserEntityUserName",
                table: "Teams",
                column: "UserEntityUserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentEntity");

            migrationBuilder.DropTable(
                name: "Encounters");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Sports");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
