using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Translators.Migrations
{
    public partial class Initialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Translators",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsHidden = table.Column<bool>(type: "bit", nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Catalogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    StartPageNumber = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Catalogs_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<long>(type: "bigint", nullable: false),
                    CatalogId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pages_Catalogs_CatalogId",
                        column: x => x.CatalogId,
                        principalTable: "Catalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Paragraphs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<long>(type: "bigint", nullable: false),
                    AnotherValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PageId = table.Column<long>(type: "bigint", nullable: false),
                    CatalogId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paragraphs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Paragraphs_Catalogs_CatalogId",
                        column: x => x.CatalogId,
                        principalTable: "Catalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Paragraphs_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Index = table.Column<int>(type: "int", nullable: false),
                    ParagraphId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Words_Paragraphs_ParagraphId",
                        column: x => x.ParagraphId,
                        principalTable: "Paragraphs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Values",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SearchValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageId = table.Column<long>(type: "bigint", nullable: false),
                    TranslatorId = table.Column<long>(type: "bigint", nullable: true),
                    TranslatorNameId = table.Column<long>(type: "bigint", nullable: true),
                    BookNameId = table.Column<long>(type: "bigint", nullable: true),
                    CategoryNameId = table.Column<long>(type: "bigint", nullable: true),
                    CatalogNameId = table.Column<long>(type: "bigint", nullable: true),
                    WordValueId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Values", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Values_Books_BookNameId",
                        column: x => x.BookNameId,
                        principalTable: "Books",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Values_Catalogs_CatalogNameId",
                        column: x => x.CatalogNameId,
                        principalTable: "Catalogs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Values_Categories_CategoryNameId",
                        column: x => x.CategoryNameId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Values_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Values_Translators_TranslatorId",
                        column: x => x.TranslatorId,
                        principalTable: "Translators",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Values_Translators_TranslatorNameId",
                        column: x => x.TranslatorNameId,
                        principalTable: "Translators",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Values_Words_WordValueId",
                        column: x => x.WordValueId,
                        principalTable: "Words",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WordLetterEntity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    WordId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordLetterEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordLetterEntity_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WordRootEntity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    WordId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordRootEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordRootEntity_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_CategoryId",
                table: "Books",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_IsHidden",
                table: "Books",
                column: "IsHidden");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_BookId",
                table: "Catalogs",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_Number",
                table: "Catalogs",
                column: "Number");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_StartPageNumber",
                table: "Catalogs",
                column: "StartPageNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Code",
                table: "Languages",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Name",
                table: "Languages",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_CatalogId",
                table: "Pages",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_Number",
                table: "Pages",
                column: "Number");

            migrationBuilder.CreateIndex(
                name: "IX_Paragraphs_CatalogId",
                table: "Paragraphs",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_Paragraphs_Number",
                table: "Paragraphs",
                column: "Number");

            migrationBuilder.CreateIndex(
                name: "IX_Paragraphs_PageId",
                table: "Paragraphs",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_Values_BookNameId",
                table: "Values",
                column: "BookNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Values_CatalogNameId",
                table: "Values",
                column: "CatalogNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Values_CategoryNameId",
                table: "Values",
                column: "CategoryNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Values_IsMain",
                table: "Values",
                column: "IsMain");

            migrationBuilder.CreateIndex(
                name: "IX_Values_LanguageId",
                table: "Values",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Values_TranslatorId",
                table: "Values",
                column: "TranslatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Values_TranslatorNameId",
                table: "Values",
                column: "TranslatorNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Values_Value",
                table: "Values",
                column: "Value");

            migrationBuilder.CreateIndex(
                name: "IX_Values_WordValueId",
                table: "Values",
                column: "WordValueId");

            migrationBuilder.CreateIndex(
                name: "IX_WordLetterEntity_Value",
                table: "WordLetterEntity",
                column: "Value");

            migrationBuilder.CreateIndex(
                name: "IX_WordLetterEntity_WordId",
                table: "WordLetterEntity",
                column: "WordId");

            migrationBuilder.CreateIndex(
                name: "IX_WordRootEntity_Value",
                table: "WordRootEntity",
                column: "Value");

            migrationBuilder.CreateIndex(
                name: "IX_WordRootEntity_WordId",
                table: "WordRootEntity",
                column: "WordId");

            migrationBuilder.CreateIndex(
                name: "IX_Words_Index",
                table: "Words",
                column: "Index");

            migrationBuilder.CreateIndex(
                name: "IX_Words_ParagraphId",
                table: "Words",
                column: "ParagraphId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Values");

            migrationBuilder.DropTable(
                name: "WordLetterEntity");

            migrationBuilder.DropTable(
                name: "WordRootEntity");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Translators");

            migrationBuilder.DropTable(
                name: "Words");

            migrationBuilder.DropTable(
                name: "Paragraphs");

            migrationBuilder.DropTable(
                name: "Pages");

            migrationBuilder.DropTable(
                name: "Catalogs");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
