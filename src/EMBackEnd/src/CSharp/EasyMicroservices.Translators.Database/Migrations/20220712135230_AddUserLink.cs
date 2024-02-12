using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Translators.Migrations
{
    public partial class AddUserLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "LinkParagraphs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_LinkParagraphs_UserId",
                table: "LinkParagraphs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LinkParagraphs_Users_UserId",
                table: "LinkParagraphs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LinkParagraphs_Users_UserId",
                table: "LinkParagraphs");

            migrationBuilder.DropIndex(
                name: "IX_LinkParagraphs_UserId",
                table: "LinkParagraphs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "LinkParagraphs");
        }
    }
}
