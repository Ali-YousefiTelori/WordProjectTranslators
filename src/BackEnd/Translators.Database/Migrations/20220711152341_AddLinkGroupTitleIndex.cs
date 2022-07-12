using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Translators.Migrations
{
    public partial class AddLinkGroupTitleIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "LinkGroups",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LinkGroups_Title",
                table: "LinkGroups",
                column: "Title");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LinkGroups_Title",
                table: "LinkGroups");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "LinkGroups",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
