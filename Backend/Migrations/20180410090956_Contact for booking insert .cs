using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class Contactforbookinginsert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "ResourceBookings");

            migrationBuilder.AddColumn<int>(
                name: "ContactId",
                table: "Bookings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ContactId",
                table: "Bookings",
                column: "ContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Contacts_ContactId",
                table: "Bookings",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Contacts_ContactId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_ContactId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ContactId",
                table: "Bookings");

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "ResourceBookings",
                nullable: false,
                defaultValue: 0);
        }
    }
}
