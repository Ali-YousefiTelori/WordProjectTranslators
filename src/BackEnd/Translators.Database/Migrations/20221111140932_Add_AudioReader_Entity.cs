using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Translators.Migrations
{
    public partial class Add_AudioReader_Entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AudioReaderId",
                table: "Audioes",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AudioReaders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AudioReaders", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Audioes_AudioReaderId",
                table: "Audioes",
                column: "AudioReaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Audioes_AudioReaders_AudioReaderId",
                table: "Audioes",
                column: "AudioReaderId",
                principalTable: "AudioReaders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audioes_AudioReaders_AudioReaderId",
                table: "Audioes");

            migrationBuilder.DropTable(
                name: "AudioReaders");

            migrationBuilder.DropIndex(
                name: "IX_Audioes_AudioReaderId",
                table: "Audioes");

            migrationBuilder.DropColumn(
                name: "AudioReaderId",
                table: "Audioes");
        }
    }
}
