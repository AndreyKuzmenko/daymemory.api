using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DayMemory.DAL.Migrations
{
    public partial class renamingimagetofile2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "NoteFile",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FileId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    NoteItemId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    OrderRank = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NoteFile_Image_FileId",
                        column: x => x.FileId,
                        principalTable: "Image",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NoteFile_NoteItem_NoteItemId",
                        column: x => x.NoteItemId,
                        principalTable: "NoteItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql("INSERT INTO NoteFile (Id, FileId, NoteItemId, OrderRank, CreatedDate, ModifiedDate) SELECT Id, FileId, NoteItemId, OrderRank, CreatedDate, ModifiedDate FROM NoteImage");

            migrationBuilder.DropTable(name: "NoteImage");


            migrationBuilder.CreateIndex(
                name: "IX_NoteFile_FileId_NoteItemId",
                table: "NoteFile",
                columns: new[] { "FileId", "NoteItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_NoteFile_NoteItemId",
                table: "NoteFile",
                column: "NoteItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NoteFile");

            migrationBuilder.CreateTable(
                name: "NoteImage",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FileId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    NoteItemId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    OrderRank = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NoteImage_Image_FileId",
                        column: x => x.FileId,
                        principalTable: "Image",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NoteImage_NoteItem_NoteItemId",
                        column: x => x.NoteItemId,
                        principalTable: "NoteItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NoteImage_FileId_NoteItemId",
                table: "NoteImage",
                columns: new[] { "FileId", "NoteItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_NoteImage_NoteItemId",
                table: "NoteImage",
                column: "NoteItemId");
        }
    }
}
