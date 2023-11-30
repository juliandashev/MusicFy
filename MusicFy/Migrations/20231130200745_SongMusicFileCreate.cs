using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicFy.Migrations
{
    /// <inheritdoc />
    public partial class SongMusicFileCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MusicFileName",
                table: "Songs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MusicFileName",
                table: "Songs");
        }
    }
}
