using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicFy.Migrations
{
    /// <inheritdoc />
    public partial class images3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Files_FileId",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_Songs_FileId",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Songs");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                table: "Songs",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                table: "Songs",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "Songs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Songs_FileId",
                table: "Songs",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Files_FileId",
                table: "Songs",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id");
        }
    }
}
