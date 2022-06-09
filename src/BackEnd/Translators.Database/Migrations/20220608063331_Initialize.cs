using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Translators.Migrations
{
    public partial class Initialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "LanguageValues",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LanguageId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LanguageValues_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_LanguageValues_NameId",
                        column: x => x.NameId,
                        principalTable: "LanguageValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_Books_LanguageValues_NameId",
                        column: x => x.NameId,
                        principalTable: "LanguageValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Catalogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    StartPageNumber = table.Column<int>(type: "int", nullable: false),
                    NameId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_Catalogs_LanguageValues_NameId",
                        column: x => x.NameId,
                        principalTable: "LanguageValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    PageId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paragraphs", x => x.Id);
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
                    ValueId = table.Column<long>(type: "bigint", nullable: false),
                    ParagraphId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Words_LanguageValues_ValueId",
                        column: x => x.ValueId,
                        principalTable: "LanguageValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Words_Paragraphs_ParagraphId",
                        column: x => x.ParagraphId,
                        principalTable: "Paragraphs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_Books_NameId",
                table: "Books",
                column: "NameId");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_BookId",
                table: "Catalogs",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_NameId",
                table: "Catalogs",
                column: "NameId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_NameId",
                table: "Categories",
                column: "NameId");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Code",
                table: "Languages",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Name",
                table: "Languages",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageValues_IsMain",
                table: "LanguageValues",
                column: "IsMain");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageValues_LanguageId",
                table: "LanguageValues",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageValues_Value",
                table: "LanguageValues",
                column: "Value");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_CatalogId",
                table: "Pages",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_Number",
                table: "Pages",
                column: "Number");

            migrationBuilder.CreateIndex(
                name: "IX_Paragraphs_Number",
                table: "Paragraphs",
                column: "Number");

            migrationBuilder.CreateIndex(
                name: "IX_Paragraphs_PageId",
                table: "Paragraphs",
                column: "PageId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Words_ValueId",
                table: "Words",
                column: "ValueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WordLetterEntity");

            migrationBuilder.DropTable(
                name: "WordRootEntity");

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

            migrationBuilder.DropTable(
                name: "LanguageValues");

            migrationBuilder.DropTable(
                name: "Languages");
        }
    }
}
