using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DayMemory.DAL.Migrations
{
    /// <inheritdoc />
    public partial class removedwidthandheightfromfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_FileItem_Id",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "FileItem");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "FileItem");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Tag",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Tag",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_FileItem_Id",
                table: "Image",
                column: "Id",
                principalTable: "FileItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_FileItem_Id",
                table: "Image");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Tag",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Tag",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "FileItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "FileItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_FileItem_Id",
                table: "Image",
                column: "Id",
                principalTable: "FileItem",
                principalColumn: "Id");
        }
    }
}
