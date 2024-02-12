using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Translators.Migrations
{
    public partial class ChangeLinkParagraphStrategy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LinkParagraphs_Paragraphs_FromParagraphId",
                table: "LinkParagraphs");

            migrationBuilder.DropForeignKey(
                name: "FK_LinkParagraphs_Paragraphs_ToParagraphId",
                table: "LinkParagraphs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LinkParagraphs",
                table: "LinkParagraphs");

            migrationBuilder.DropIndex(
                name: "IX_LinkParagraphs_LinkGroupId",
                table: "LinkParagraphs");

            migrationBuilder.DropColumn(
                name: "FromParagraphId",
                table: "LinkParagraphs");

            migrationBuilder.RenameColumn(
                name: "ToParagraphId",
                table: "LinkParagraphs",
                newName: "ParagraphId");

            migrationBuilder.RenameIndex(
                name: "IX_LinkParagraphs_ToParagraphId",
                table: "LinkParagraphs",
                newName: "IX_LinkParagraphs_ParagraphId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LinkParagraphs",
                table: "LinkParagraphs",
                columns: new[] { "LinkGroupId", "ParagraphId" });

            migrationBuilder.AddForeignKey(
                name: "FK_LinkParagraphs_Paragraphs_ParagraphId",
                table: "LinkParagraphs",
                column: "ParagraphId",
                principalTable: "Paragraphs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LinkParagraphs_Paragraphs_ParagraphId",
                table: "LinkParagraphs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LinkParagraphs",
                table: "LinkParagraphs");

            migrationBuilder.RenameColumn(
                name: "ParagraphId",
                table: "LinkParagraphs",
                newName: "ToParagraphId");

            migrationBuilder.RenameIndex(
                name: "IX_LinkParagraphs_ParagraphId",
                table: "LinkParagraphs",
                newName: "IX_LinkParagraphs_ToParagraphId");

            migrationBuilder.AddColumn<long>(
                name: "FromParagraphId",
                table: "LinkParagraphs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LinkParagraphs",
                table: "LinkParagraphs",
                columns: new[] { "FromParagraphId", "ToParagraphId" });

            migrationBuilder.CreateIndex(
                name: "IX_LinkParagraphs_LinkGroupId",
                table: "LinkParagraphs",
                column: "LinkGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_LinkParagraphs_Paragraphs_FromParagraphId",
                table: "LinkParagraphs",
                column: "FromParagraphId",
                principalTable: "Paragraphs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LinkParagraphs_Paragraphs_ToParagraphId",
                table: "LinkParagraphs",
                column: "ToParagraphId",
                principalTable: "Paragraphs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
