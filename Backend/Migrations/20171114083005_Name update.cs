using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class Nameupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Package_FK_Package",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Branch_Presentations_FK_Branch",
                table: "Branch");

            migrationBuilder.DropForeignKey(
                name: "FK_Branch_Bookings_FK_Branches",
                table: "Branch");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Package",
                table: "Package");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Branch",
                table: "Branch");

            migrationBuilder.RenameTable(
                name: "Package",
                newName: "Packages");

            migrationBuilder.RenameTable(
                name: "Branch",
                newName: "Branches");

            migrationBuilder.RenameIndex(
                name: "IX_Branch_FK_Branches",
                table: "Branches",
                newName: "IX_Branches_FK_Branches");

            migrationBuilder.RenameIndex(
                name: "IX_Branch_FK_Branch",
                table: "Branches",
                newName: "IX_Branches_FK_Branch");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Packages",
                table: "Packages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Branches",
                table: "Branches",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Packages_FK_Package",
                table: "Bookings",
                column: "FK_Package",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_Presentations_FK_Branch",
                table: "Branches",
                column: "FK_Branch",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Packages_FK_Package",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Presentations_FK_Branch",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Bookings_FK_Branches",
                table: "Branches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Packages",
                table: "Packages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Branches",
                table: "Branches");

            migrationBuilder.RenameTable(
                name: "Packages",
                newName: "Package");

            migrationBuilder.RenameTable(
                name: "Branches",
                newName: "Branch");

            migrationBuilder.RenameIndex(
                name: "IX_Branches_FK_Branches",
                table: "Branch",
                newName: "IX_Branch_FK_Branches");

            migrationBuilder.RenameIndex(
                name: "IX_Branches_FK_Branch",
                table: "Branch",
                newName: "IX_Branch_FK_Branch");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Package",
                table: "Package",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Branch",
                table: "Branch",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Package_FK_Package",
                table: "Bookings",
                column: "FK_Package",
                principalTable: "Package",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Branch_Presentations_FK_Branch",
                table: "Branch",
                column: "FK_Branch",
                principalTable: "Presentations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Branch_Bookings_FK_Branches",
                table: "Branch",
                column: "FK_Branches",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
