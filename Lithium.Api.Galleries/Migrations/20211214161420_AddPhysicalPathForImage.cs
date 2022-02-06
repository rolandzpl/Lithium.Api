using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lithium.Api.Galleries.Migrations
{
    public partial class AddPhysicalPathForImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhysicalPath",
                table: "GalleryImage",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhysicalPath",
                table: "GalleryImage");
        }
    }
}
