using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Translators.Migrations
{
    public partial class Add_Language_TranslatorEntity_For_AudioEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "Audioes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "LanguageId",
                table: "Audioes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TranslatorId",
                table: "Audioes",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Audioes_LanguageId",
                table: "Audioes",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Audioes_TranslatorId",
                table: "Audioes",
                column: "TranslatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Audioes_Languages_LanguageId",
                table: "Audioes",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Audioes_Translators_TranslatorId",
                table: "Audioes",
                column: "TranslatorId",
                principalTable: "Translators",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audioes_Languages_LanguageId",
                table: "Audioes");

            migrationBuilder.DropForeignKey(
                name: "FK_Audioes_Translators_TranslatorId",
                table: "Audioes");

            migrationBuilder.DropIndex(
                name: "IX_Audioes_LanguageId",
                table: "Audioes");

            migrationBuilder.DropIndex(
                name: "IX_Audioes_TranslatorId",
                table: "Audioes");

            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "Audioes");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "Audioes");

            migrationBuilder.DropColumn(
                name: "TranslatorId",
                table: "Audioes");
        }
    }
}
