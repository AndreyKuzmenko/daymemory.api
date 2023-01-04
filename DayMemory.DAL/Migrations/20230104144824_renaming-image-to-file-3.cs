using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DayMemory.DAL.Migrations
{
    public partial class renamingimagetofile3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteFile_Image_FileId",
                table: "NoteFile");

            migrationBuilder.CreateTable(
                name: "FileItem",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FileContentType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FileSize = table.Column<int>(type: "int", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileItem_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.Sql("INSERT INTO FileItem (Id, FileName, FileContentType, FileSize, Width, Height, UserId, CreatedDate, ModifiedDate) SELECT Id, FileName, FileContentType, FileSize, Width, Height, UserId, CreatedDate, ModifiedDate FROM Image");

            migrationBuilder.DropTable(name: "Image");

            migrationBuilder.CreateIndex(
                name: "IX_FileItem_UserId",
                table: "FileItem",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteFile_File_FileId",
                table: "NoteFile",
                column: "FileId",
                principalTable: "FileItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_FileItem_AspNetUsers_UserId",
            //    table: "FileItem");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_NoteFile_FileItem_FileId",
            //    table: "NoteFile");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_FileItem",
            //    table: "FileItem");

            //migrationBuilder.RenameTable(
            //    name: "FileItem",
            //    newName: "Image");

            //migrationBuilder.RenameIndex(
            //    name: "IX_FileItem_UserId",
            //    table: "Image",
            //    newName: "IX_Image_UserId");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_Image",
            //    table: "Image",
            //    column: "Id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Image_AspNetUsers_UserId",
            //    table: "Image",
            //    column: "UserId",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_NoteFile_Image_FileId",
            //    table: "NoteFile",
            //    column: "FileId",
            //    principalTable: "Image",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
