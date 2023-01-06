using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DayMemory.DAL.Migrations
{
    public partial class migrationtoimagetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE FileItem set FileType = 1");

            migrationBuilder.Sql("INSERT INTO [IMAGE]   ([Image].Id, [Image].ImageWidth, [Image].ImageHeight) SELECT FileItem.Id, FileItem.Width, FileItem.Height FROM FileItem");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
