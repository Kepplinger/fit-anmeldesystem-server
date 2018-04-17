using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class addedfklocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Locations_LocationId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "Bookings",
                newName: "FK_Location");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_LocationId",
                table: "Bookings",
                newName: "IX_Bookings_FK_Location");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Locations_FK_Location",
                table: "Bookings",
                column: "FK_Location",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Locations_FK_Location",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "FK_Location",
                table: "Bookings",
                newName: "LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_FK_Location",
                table: "Bookings",
                newName: "IX_Bookings_LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Locations_LocationId",
                table: "Bookings",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
