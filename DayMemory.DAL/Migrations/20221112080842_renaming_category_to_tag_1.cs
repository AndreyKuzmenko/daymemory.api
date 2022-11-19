using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DayMemory.DAL.Migrations
{
    public partial class renaming_category_to_tag_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteItem_Category_CategoryId",
                table: "NoteItem");

            migrationBuilder.DropIndex(
                name: "IX_NoteItem_CategoryId",
                table: "NoteItem");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "NoteItem");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "NoteItem",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NoteItem_CategoryId",
                table: "NoteItem",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteItem_Category_CategoryId",
                table: "NoteItem",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id");
        }
    }
}
