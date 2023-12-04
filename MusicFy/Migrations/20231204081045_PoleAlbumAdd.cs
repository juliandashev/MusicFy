using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicFy.Migrations
{
    /// <inheritdoc />
    public partial class PoleAlbumAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Pole",
                table: "Albums",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pole",
                table: "Albums");
        }
    }
}
