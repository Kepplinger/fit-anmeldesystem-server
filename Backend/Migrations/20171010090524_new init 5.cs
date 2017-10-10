using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class newinit5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Areas_Events_FK_Event",
                table: "Areas");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Categorys_FK_Category",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Companies_FK_Company",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Events_FK_Event",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Locations_FK_Location",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Presentations_FK_Presentation",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Categorys_Locations_Fk_Location",
                table: "Categorys");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Addresses_FK_Address",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Contacts_FK_Contact",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Persons_FK_Person",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailAllocations_Bookings_FK_Booking",
                table: "DetailAllocations");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailAllocations_Details_FK_Detail",
                table: "DetailAllocations");

            migrationBuilder.DropForeignKey(
                name: "FK_Lecturers_Persons_FK_Person",
                table: "Lecturers");

            migrationBuilder.DropForeignKey(
                name: "FK_Lecturers_Presentations_FK_Presentation",
                table: "Lecturers");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Areas_FK_Area",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Rerpresentatives_Bookings_FK_Booking",
                table: "Rerpresentatives");

            migrationBuilder.DropForeignKey(
                name: "FK_Rerpresentatives_Persons_FK_Person",
                table: "Rerpresentatives");

            migrationBuilder.DropForeignKey(
                name: "FK_ResourceBookings_Bookings_FK_Booking",
                table: "ResourceBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_ResourceBookings_Resources_FK_Resource",
                table: "ResourceBookings");

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_Events_FK_Event",
                table: "Areas",
                column: "FK_Event",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Categorys_FK_Category",
                table: "Bookings",
                column: "FK_Category",
                principalTable: "Categorys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Companies_FK_Company",
                table: "Bookings",
                column: "FK_Company",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Events_FK_Event",
                table: "Bookings",
                column: "FK_Event",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Locations_FK_Location",
                table: "Bookings",
                column: "FK_Location",
                principalTable: "Locations",
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
                name: "FK_Categorys_Locations_Fk_Location",
                table: "Categorys",
                column: "Fk_Location",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Addresses_FK_Address",
                table: "Companies",
                column: "FK_Address",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Contacts_FK_Contact",
                table: "Companies",
                column: "FK_Contact",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Persons_FK_Person",
                table: "Contacts",
                column: "FK_Person",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailAllocations_Bookings_FK_Booking",
                table: "DetailAllocations",
                column: "FK_Booking",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailAllocations_Details_FK_Detail",
                table: "DetailAllocations",
                column: "FK_Detail",
                principalTable: "Details",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lecturers_Persons_FK_Person",
                table: "Lecturers",
                column: "FK_Person",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lecturers_Presentations_FK_Presentation",
                table: "Lecturers",
                column: "FK_Presentation",
                principalTable: "Presentations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Areas_FK_Area",
                table: "Locations",
                column: "FK_Area",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rerpresentatives_Bookings_FK_Booking",
                table: "Rerpresentatives",
                column: "FK_Booking",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rerpresentatives_Persons_FK_Person",
                table: "Rerpresentatives",
                column: "FK_Person",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceBookings_Bookings_FK_Booking",
                table: "ResourceBookings",
                column: "FK_Booking",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceBookings_Resources_FK_Resource",
                table: "ResourceBookings",
                column: "FK_Resource",
                principalTable: "Resources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Areas_Events_FK_Event",
                table: "Areas");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Categorys_FK_Category",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Companies_FK_Company",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Events_FK_Event",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Locations_FK_Location",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Presentations_FK_Presentation",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Categorys_Locations_Fk_Location",
                table: "Categorys");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Addresses_FK_Address",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Contacts_FK_Contact",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Persons_FK_Person",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailAllocations_Bookings_FK_Booking",
                table: "DetailAllocations");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailAllocations_Details_FK_Detail",
                table: "DetailAllocations");

            migrationBuilder.DropForeignKey(
                name: "FK_Lecturers_Persons_FK_Person",
                table: "Lecturers");

            migrationBuilder.DropForeignKey(
                name: "FK_Lecturers_Presentations_FK_Presentation",
                table: "Lecturers");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Areas_FK_Area",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Rerpresentatives_Bookings_FK_Booking",
                table: "Rerpresentatives");

            migrationBuilder.DropForeignKey(
                name: "FK_Rerpresentatives_Persons_FK_Person",
                table: "Rerpresentatives");

            migrationBuilder.DropForeignKey(
                name: "FK_ResourceBookings_Bookings_FK_Booking",
                table: "ResourceBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_ResourceBookings_Resources_FK_Resource",
                table: "ResourceBookings");

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_Events_FK_Event",
                table: "Areas",
                column: "FK_Event",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Categorys_FK_Category",
                table: "Bookings",
                column: "FK_Category",
                principalTable: "Categorys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Companies_FK_Company",
                table: "Bookings",
                column: "FK_Company",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Events_FK_Event",
                table: "Bookings",
                column: "FK_Event",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Locations_FK_Location",
                table: "Bookings",
                column: "FK_Location",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Presentations_FK_Presentation",
                table: "Bookings",
                column: "FK_Presentation",
                principalTable: "Presentations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Categorys_Locations_Fk_Location",
                table: "Categorys",
                column: "Fk_Location",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Addresses_FK_Address",
                table: "Companies",
                column: "FK_Address",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Contacts_FK_Contact",
                table: "Companies",
                column: "FK_Contact",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Persons_FK_Person",
                table: "Contacts",
                column: "FK_Person",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailAllocations_Bookings_FK_Booking",
                table: "DetailAllocations",
                column: "FK_Booking",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailAllocations_Details_FK_Detail",
                table: "DetailAllocations",
                column: "FK_Detail",
                principalTable: "Details",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lecturers_Persons_FK_Person",
                table: "Lecturers",
                column: "FK_Person",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lecturers_Presentations_FK_Presentation",
                table: "Lecturers",
                column: "FK_Presentation",
                principalTable: "Presentations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Areas_FK_Area",
                table: "Locations",
                column: "FK_Area",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rerpresentatives_Bookings_FK_Booking",
                table: "Rerpresentatives",
                column: "FK_Booking",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rerpresentatives_Persons_FK_Person",
                table: "Rerpresentatives",
                column: "FK_Person",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceBookings_Bookings_FK_Booking",
                table: "ResourceBookings",
                column: "FK_Booking",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceBookings_Resources_FK_Resource",
                table: "ResourceBookings",
                column: "FK_Resource",
                principalTable: "Resources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
