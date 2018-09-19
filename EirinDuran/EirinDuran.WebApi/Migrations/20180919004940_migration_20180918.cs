using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EirinDuran.WebApi.Migrations
{
    public partial class migration_20180918 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserEntityId",
                table: "Teams",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CommentEntity",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: true),
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
                        name: "FK_CommentEntity_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teams_UserEntityId",
                table: "Teams",
                column: "UserEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentEntity_EncounterEntityId",
                table: "CommentEntity",
                column: "EncounterEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentEntity_UserId",
                table: "CommentEntity",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Users_UserEntityId",
                table: "Teams",
                column: "UserEntityId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Users_UserEntityId",
                table: "Teams");

            migrationBuilder.DropTable(
                name: "CommentEntity");

            migrationBuilder.DropIndex(
                name: "IX_Teams_UserEntityId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "UserEntityId",
                table: "Teams");
        }
    }
}
