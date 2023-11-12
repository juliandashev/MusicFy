using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicFy.Migrations
{
    /// <inheritdoc />
    public partial class FileSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Songs");

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Songs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Image_id",
                table: "Songs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Songs_ImageId",
                table: "Songs",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Images_ImageId",
                table: "Songs",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Images_ImageId",
                table: "Songs");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Songs_ImageId",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "Image_id",
                table: "Songs");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Songs",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
