using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class final3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Areas_Events_FK_Event",
                table: "Areas");

            migrationBuilder.DropIndex(
                name: "IX_Areas_FK_Event",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "FK_Event",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "Tag",
                table: "Packages");

            migrationBuilder.AddColumn<int>(
                name: "Discriminator",
                table: "Packages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Packages");

            migrationBuilder.AddColumn<int>(
                name: "FK_Event",
                table: "Areas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Packages",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Tag",
                table: "Packages",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Areas_FK_Event",
                table: "Areas",
                column: "FK_Event");

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_Events_FK_Event",
                table: "Areas",
                column: "FK_Event",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
