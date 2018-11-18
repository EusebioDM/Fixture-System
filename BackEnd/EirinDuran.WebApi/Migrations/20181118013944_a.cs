using Microsoft.EntityFrameworkCore.Migrations;

namespace EirinDuran.WebApi.Migrations
{
    public partial class a : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentEntity_Encounters_EncounterEntityId",
                table: "CommentEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentEntity_Users_UserName",
                table: "CommentEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentEntity",
                table: "CommentEntity");

            migrationBuilder.RenameTable(
                name: "CommentEntity",
                newName: "Comments");

            migrationBuilder.RenameIndex(
                name: "IX_CommentEntity_UserName",
                table: "Comments",
                newName: "IX_Comments_UserName");

            migrationBuilder.RenameIndex(
                name: "IX_CommentEntity_EncounterEntityId",
                table: "Comments",
                newName: "IX_Comments_EncounterEntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Encounters_EncounterEntityId",
                table: "Comments",
                column: "EncounterEntityId",
                principalTable: "Encounters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserName",
                table: "Comments",
                column: "UserName",
                principalTable: "Users",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Encounters_EncounterEntityId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserName",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "CommentEntity");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_UserName",
                table: "CommentEntity",
                newName: "IX_CommentEntity_UserName");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_EncounterEntityId",
                table: "CommentEntity",
                newName: "IX_CommentEntity_EncounterEntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentEntity",
                table: "CommentEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentEntity_Encounters_EncounterEntityId",
                table: "CommentEntity",
                column: "EncounterEntityId",
                principalTable: "Encounters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentEntity_Users_UserName",
                table: "CommentEntity",
                column: "UserName",
                principalTable: "Users",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
