using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Translators.Migrations
{
    public partial class AddApplicationType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "ApplicationType",
                table: "AppVersions",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationType",
                table: "AppVersions");
        }
    }
}
