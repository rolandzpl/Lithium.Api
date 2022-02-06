using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lithium.Api.Galleries.Migrations
{
    public partial class GalleryImageDataExtension : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "GalleryImage",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageType",
                table: "GalleryImage",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "GalleryImage",
                type: "INTEGER",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "GalleryImage");

            migrationBuilder.DropColumn(
                name: "ImageType",
                table: "GalleryImage");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "GalleryImage");
        }
    }
}
