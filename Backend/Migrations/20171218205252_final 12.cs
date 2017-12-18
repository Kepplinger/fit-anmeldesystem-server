using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class final12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FK_Resources",
                table: "Resources",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FK_Resources",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Resources_FK_Resources",
                table: "Resources",
                column: "FK_Resources");

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_Bookings_FK_Resources",
                table: "Resources",
                column: "FK_Resources",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resources_Bookings_FK_Resources",
                table: "Resources");

            migrationBuilder.DropIndex(
                name: "IX_Resources_FK_Resources",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "FK_Resources",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "FK_Resources",
                table: "Bookings");
        }
    }
}
