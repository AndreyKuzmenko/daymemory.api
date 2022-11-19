using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DayMemory.DAL.Migrations
{
    public partial class delete_questions_with_list : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_AspNetUsers_UserId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_QuestionList_QuestionListId",
                table: "Question");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_AspNetUsers_UserId",
                table: "Question",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_QuestionList_QuestionListId",
                table: "Question",
                column: "QuestionListId",
                principalTable: "QuestionList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_AspNetUsers_UserId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_QuestionList_QuestionListId",
                table: "Question");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_AspNetUsers_UserId",
                table: "Question",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_QuestionList_QuestionListId",
                table: "Question",
                column: "QuestionListId",
                principalTable: "QuestionList",
                principalColumn: "Id");
        }
    }
}
