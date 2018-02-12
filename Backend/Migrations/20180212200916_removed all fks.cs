using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class removedallfks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Events_FK_Event",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Presentations_FK_Presentation",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Bookings_FK_Branches",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Resources_Bookings_FK_Resources",
                table: "Resources");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_FK_Event",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "FK_Event",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "FK_Resources",
                table: "Resources",
                newName: "BookingId");

            migrationBuilder.RenameIndex(
                name: "IX_Resources_FK_Resources",
                table: "Resources",
                newName: "IX_Resources_BookingId");

            migrationBuilder.RenameColumn(
                name: "FK_Branches",
                table: "Branches",
                newName: "BookingId");

            migrationBuilder.RenameIndex(
                name: "IX_Branches_FK_Branches",
                table: "Branches",
                newName: "IX_Branches_BookingId");

            migrationBuilder.RenameColumn(
                name: "FK_Presentation",
                table: "Bookings",
                newName: "PresentationId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_FK_Presentation",
                table: "Bookings",
                newName: "IX_Bookings_PresentationId");

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Bookings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_EventId",
                table: "Bookings",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Events_EventId",
                table: "Bookings",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Presentations_PresentationId",
                table: "Bookings",
                column: "PresentationId",
                principalTable: "Presentations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_Bookings_BookingId",
                table: "Branches",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_Bookings_BookingId",
                table: "Resources",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Events_EventId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Presentations_PresentationId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Bookings_BookingId",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Resources_Bookings_BookingId",
                table: "Resources");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_EventId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "BookingId",
                table: "Resources",
                newName: "FK_Resources");

            migrationBuilder.RenameIndex(
                name: "IX_Resources_BookingId",
                table: "Resources",
                newName: "IX_Resources_FK_Resources");

            migrationBuilder.RenameColumn(
                name: "BookingId",
                table: "Branches",
                newName: "FK_Branches");

            migrationBuilder.RenameIndex(
                name: "IX_Branches_BookingId",
                table: "Branches",
                newName: "IX_Branches_FK_Branches");

            migrationBuilder.RenameColumn(
                name: "PresentationId",
                table: "Bookings",
                newName: "FK_Presentation");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_PresentationId",
                table: "Bookings",
                newName: "IX_Bookings_FK_Presentation");

            migrationBuilder.AddColumn<int>(
                name: "FK_Event",
                table: "Bookings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_FK_Event",
                table: "Bookings",
                column: "FK_Event");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Events_FK_Event",
                table: "Bookings",
                column: "FK_Event",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Presentations_FK_Presentation",
                table: "Bookings",
                column: "FK_Presentation",
                principalTable: "Presentations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_Bookings_FK_Branches",
                table: "Branches",
                column: "FK_Branches",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_Bookings_FK_Resources",
                table: "Resources",
                column: "FK_Resources",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
