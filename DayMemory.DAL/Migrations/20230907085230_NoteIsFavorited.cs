using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DayMemory.DAL.Migrations
{
    /// <inheritdoc />
    public partial class NoteIsFavorited : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFavorited",
                table: "NoteItem",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFavorited",
                table: "NoteItem");
        }
    }
}
