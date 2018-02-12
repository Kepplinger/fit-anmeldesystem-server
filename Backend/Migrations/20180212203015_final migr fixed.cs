using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class finalmigrfixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Companies_CompanyId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Events_EventId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Packages_FitPackageId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Presentations_PresentationId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_CompanyId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_EventId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_FitPackageId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "FitPackageId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "PresentationId",
                table: "Bookings",
                newName: "FK_Presentation");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_PresentationId",
                table: "Bookings",
                newName: "IX_Bookings_FK_Presentation");

            migrationBuilder.AddColumn<int>(
                name: "FK_Company",
                table: "Bookings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FK_Event",
                table: "Bookings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FK_FitPackage",
                table: "Bookings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_FK_Company",
                table: "Bookings",
                column: "FK_Company");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_FK_Event",
                table: "Bookings",
                column: "FK_Event");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_FK_FitPackage",
                table: "Bookings",
                column: "FK_FitPackage");

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
                name: "FK_Bookings_Packages_FK_FitPackage",
                table: "Bookings",
                column: "FK_FitPackage",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Presentations_FK_Presentation",
                table: "Bookings",
                column: "FK_Presentation",
                principalTable: "Presentations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Companies_FK_Company",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Events_FK_Event",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Packages_FK_FitPackage",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Presentations_FK_Presentation",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_FK_Company",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_FK_Event",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_FK_FitPackage",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "FK_Company",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "FK_Event",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "FK_FitPackage",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "FK_Presentation",
                table: "Bookings",
                newName: "PresentationId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_FK_Presentation",
                table: "Bookings",
                newName: "IX_Bookings_PresentationId");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Bookings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Bookings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FitPackageId",
                table: "Bookings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CompanyId",
                table: "Bookings",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_EventId",
                table: "Bookings",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_FitPackageId",
                table: "Bookings",
                column: "FitPackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Companies_CompanyId",
                table: "Bookings",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Events_EventId",
                table: "Bookings",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Packages_FitPackageId",
                table: "Bookings",
                column: "FitPackageId",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Presentations_PresentationId",
                table: "Bookings",
                column: "PresentationId",
                principalTable: "Presentations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
