using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class final2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "addition",
                table: "Addresses",
                newName: "Addition");

            migrationBuilder.AddColumn<int>(
                name: "FK_Areas",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FK_Areas",
                table: "Areas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Areas_FK_Areas",
                table: "Areas",
                column: "FK_Areas");

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_Events_FK_Areas",
                table: "Areas",
                column: "FK_Areas",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Areas_Events_FK_Areas",
                table: "Areas");

            migrationBuilder.DropIndex(
                name: "IX_Areas_FK_Areas",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "FK_Areas",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "FK_Areas",
                table: "Areas");

            migrationBuilder.RenameColumn(
                name: "Addition",
                table: "Addresses",
                newName: "addition");
        }
    }
}
