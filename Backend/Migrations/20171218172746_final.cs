using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class final : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rerpresentatives_Bookings_FK_Booking",
                table: "Rerpresentatives");

            migrationBuilder.DropIndex(
                name: "IX_Rerpresentatives_FK_Booking",
                table: "Rerpresentatives");

            migrationBuilder.DropColumn(
                name: "FK_Booking",
                table: "Rerpresentatives");

            migrationBuilder.DropColumn(
                name: "AddressAdditional",
                table: "Addresses");

            migrationBuilder.AddColumn<string>(
                name: "addition",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "addition",
                table: "Addresses");

            migrationBuilder.AddColumn<int>(
                name: "FK_Booking",
                table: "Rerpresentatives",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AddressAdditional",
                table: "Addresses",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rerpresentatives_FK_Booking",
                table: "Rerpresentatives",
                column: "FK_Booking");

            migrationBuilder.AddForeignKey(
                name: "FK_Rerpresentatives_Bookings_FK_Booking",
                table: "Rerpresentatives",
                column: "FK_Booking",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
