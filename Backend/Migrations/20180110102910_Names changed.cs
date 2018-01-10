using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class Nameschanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Rerpresentatives",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "LogoUrl",
                table: "Companies",
                newName: "Logo");

            migrationBuilder.RenameColumn(
                name: "GraphicURL",
                table: "Areas",
                newName: "Graphic");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Rerpresentatives",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "Logo",
                table: "Companies",
                newName: "LogoUrl");

            migrationBuilder.RenameColumn(
                name: "Graphic",
                table: "Areas",
                newName: "GraphicURL");
        }
    }
}
