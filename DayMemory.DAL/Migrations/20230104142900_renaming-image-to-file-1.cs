using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DayMemory.DAL.Migrations
{
    public partial class renamingimagetofile1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteImage_Image_ImageId",
                table: "NoteImage");

            migrationBuilder.RenameColumn(
                name: "ImageId",
                table: "NoteImage",
                newName: "FileId");

            migrationBuilder.RenameIndex(
                name: "IX_NoteImage_ImageId_NoteItemId",
                table: "NoteImage",
                newName: "IX_NoteImage_FileId_NoteItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteImage_Image_FileId",
                table: "NoteImage",
                column: "FileId",
                principalTable: "Image",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteImage_Image_FileId",
                table: "NoteImage");

            migrationBuilder.RenameColumn(
                name: "FileId",
                table: "NoteImage",
                newName: "ImageId");

            migrationBuilder.RenameIndex(
                name: "IX_NoteImage_FileId_NoteItemId",
                table: "NoteImage",
                newName: "IX_NoteImage_ImageId_NoteItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteImage_Image_ImageId",
                table: "NoteImage",
                column: "ImageId",
                principalTable: "Image",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
