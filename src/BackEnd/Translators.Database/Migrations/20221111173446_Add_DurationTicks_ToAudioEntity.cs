using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Translators.Migrations
{
    public partial class Add_DurationTicks_ToAudioEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DurationTicks",
                table: "Audioes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationTicks",
                table: "Audioes");
        }
    }
}
