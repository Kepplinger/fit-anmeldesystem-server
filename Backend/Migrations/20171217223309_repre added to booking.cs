using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class repreaddedtobooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubjectAreas",
                table: "Companies");

            migrationBuilder.AddColumn<int>(
                name: "FK_Representatives",
                table: "Rerpresentatives",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FK_Representatives",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rerpresentatives_FK_Representatives",
                table: "Rerpresentatives",
                column: "FK_Representatives");

            migrationBuilder.AddForeignKey(
                name: "FK_Rerpresentatives_Bookings_FK_Representatives",
                table: "Rerpresentatives",
                column: "FK_Representatives",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rerpresentatives_Bookings_FK_Representatives",
                table: "Rerpresentatives");

            migrationBuilder.DropIndex(
                name: "IX_Rerpresentatives_FK_Representatives",
                table: "Rerpresentatives");

            migrationBuilder.DropColumn(
                name: "FK_Representatives",
                table: "Rerpresentatives");

            migrationBuilder.DropColumn(
                name: "FK_Representatives",
                table: "Bookings");

            migrationBuilder.AddColumn<string>(
                name: "SubjectAreas",
                table: "Companies",
                nullable: true);
        }
    }
}
