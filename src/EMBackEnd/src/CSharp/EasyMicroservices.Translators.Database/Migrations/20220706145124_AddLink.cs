using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Translators.Migrations
{
    public partial class AddLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LinkGroups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LinkParagraphs",
                columns: table => new
                {
                    FromParagraphId = table.Column<long>(type: "bigint", nullable: false),
                    ToParagraphId = table.Column<long>(type: "bigint", nullable: false),
                    LinkGroupId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkParagraphs", x => new { x.FromParagraphId, x.ToParagraphId });
                    table.ForeignKey(
                        name: "FK_LinkParagraphs_LinkGroups_LinkGroupId",
                        column: x => x.LinkGroupId,
                        principalTable: "LinkGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LinkParagraphs_Paragraphs_FromParagraphId",
                        column: x => x.FromParagraphId,
                        principalTable: "Paragraphs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LinkParagraphs_Paragraphs_ToParagraphId",
                        column: x => x.ToParagraphId,
                        principalTable: "Paragraphs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LinkParagraphs_LinkGroupId",
                table: "LinkParagraphs",
                column: "LinkGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkParagraphs_ToParagraphId",
                table: "LinkParagraphs",
                column: "ToParagraphId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LinkParagraphs");

            migrationBuilder.DropTable(
                name: "LinkGroups");
        }
    }
}
