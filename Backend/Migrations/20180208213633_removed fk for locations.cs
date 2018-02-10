using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class removedfkforlocations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Locations_FK_Location",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_FK_Location",
                table: "Bookings");

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Bookings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_LocationId",
                table: "Bookings",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Locations_LocationId",
                table: "Bookings",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Locations_LocationId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_LocationId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_FK_Location",
                table: "Bookings",
                column: "FK_Location");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Locations_FK_Location",
                table: "Bookings",
                column: "FK_Location",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
