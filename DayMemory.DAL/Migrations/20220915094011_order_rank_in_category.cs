﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DayMemory.DAL.Migrations
{
    public partial class order_rank_in_category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderRank",
                table: "Category",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderRank",
                table: "Category");
        }
    }
}
