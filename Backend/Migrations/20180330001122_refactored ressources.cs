using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class refactoredressources : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resources_Bookings_BookingId",
                table: "Resources");

            migrationBuilder.DropIndex(
                name: "IX_Resources_BookingId",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "Resources");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "Resources",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resources_BookingId",
                table: "Resources",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_Bookings_BookingId",
                table: "Resources",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
