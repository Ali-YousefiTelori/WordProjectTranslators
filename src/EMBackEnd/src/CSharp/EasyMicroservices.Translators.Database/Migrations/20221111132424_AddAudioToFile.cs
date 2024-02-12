using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Translators.Migrations
{
    public partial class AddAudioToFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audioes_Languages_LanguageId",
                table: "Audioes");

            migrationBuilder.DropForeignKey(
                name: "FK_Audioes_Pages_PageId",
                table: "Audioes");

            migrationBuilder.DropForeignKey(
                name: "FK_Audioes_Translators_TranslatorId",
                table: "Audioes");

            migrationBuilder.AddColumn<long>(
                name: "ParagraphId",
                table: "Audioes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Audioes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Audioes_ParagraphId",
                table: "Audioes",
                column: "ParagraphId");

            migrationBuilder.AddForeignKey(
                name: "FK_Audioes_Languages_LanguageId",
                table: "Audioes",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Audioes_Pages_PageId",
                table: "Audioes",
                column: "PageId",
                principalTable: "Pages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Audioes_Paragraphs_ParagraphId",
                table: "Audioes",
                column: "ParagraphId",
                principalTable: "Paragraphs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Audioes_Translators_TranslatorId",
                table: "Audioes",
                column: "TranslatorId",
                principalTable: "Translators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audioes_Languages_LanguageId",
                table: "Audioes");

            migrationBuilder.DropForeignKey(
                name: "FK_Audioes_Pages_PageId",
                table: "Audioes");

            migrationBuilder.DropForeignKey(
                name: "FK_Audioes_Paragraphs_ParagraphId",
                table: "Audioes");

            migrationBuilder.DropForeignKey(
                name: "FK_Audioes_Translators_TranslatorId",
                table: "Audioes");

            migrationBuilder.DropIndex(
                name: "IX_Audioes_ParagraphId",
                table: "Audioes");

            migrationBuilder.DropColumn(
                name: "ParagraphId",
                table: "Audioes");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Audioes");

            migrationBuilder.AddForeignKey(
                name: "FK_Audioes_Languages_LanguageId",
                table: "Audioes",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Audioes_Pages_PageId",
                table: "Audioes",
                column: "PageId",
                principalTable: "Pages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Audioes_Translators_TranslatorId",
                table: "Audioes",
                column: "TranslatorId",
                principalTable: "Translators",
                principalColumn: "Id");
        }
    }
}
