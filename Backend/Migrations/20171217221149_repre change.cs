using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class reprechange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "Rerpresentatives");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Rerpresentatives",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Rerpresentatives");

            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "Rerpresentatives",
                nullable: true);
        }
    }
}
