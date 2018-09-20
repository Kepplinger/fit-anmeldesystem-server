using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class ForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_AspNetUsers_FitUserId",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "FitUserId",
                table: "Companies",
                newName: "fk_FitUser");

            migrationBuilder.RenameIndex(
                name: "IX_Companies_FitUserId",
                table: "Companies",
                newName: "IX_Companies_fk_FitUser");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_AspNetUsers_fk_FitUser",
                table: "Companies",
                column: "fk_FitUser",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_AspNetUsers_fk_FitUser",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "fk_FitUser",
                table: "Companies",
                newName: "FitUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Companies_fk_FitUser",
                table: "Companies",
                newName: "IX_Companies_FitUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_AspNetUsers_FitUserId",
                table: "Companies",
                column: "FitUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
