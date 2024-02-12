using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Translators.Migrations
{
    public partial class Add_CategoryId_To_Readings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                table: "Readings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Readings_CategoryId",
                table: "Readings",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Readings_Categories_CategoryId",
                table: "Readings",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Readings_Categories_CategoryId",
                table: "Readings");

            migrationBuilder.DropIndex(
                name: "IX_Readings_CategoryId",
                table: "Readings");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Readings");
        }
    }
}
