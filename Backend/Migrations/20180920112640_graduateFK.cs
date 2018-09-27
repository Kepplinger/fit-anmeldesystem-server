using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class graduateFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Graduates_AspNetUsers_FitUserId",
                table: "Graduates");

            migrationBuilder.RenameColumn(
                name: "FitUserId",
                table: "Graduates",
                newName: "fk_FitUser");

            migrationBuilder.RenameIndex(
                name: "IX_Graduates_FitUserId",
                table: "Graduates",
                newName: "IX_Graduates_fk_FitUser");

            migrationBuilder.AddForeignKey(
                name: "FK_Graduates_AspNetUsers_fk_FitUser",
                table: "Graduates",
                column: "fk_FitUser",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Graduates_AspNetUsers_fk_FitUser",
                table: "Graduates");

            migrationBuilder.RenameColumn(
                name: "fk_FitUser",
                table: "Graduates",
                newName: "FitUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Graduates_fk_FitUser",
                table: "Graduates",
                newName: "IX_Graduates_FitUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Graduates_AspNetUsers_FitUserId",
                table: "Graduates",
                column: "FitUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
