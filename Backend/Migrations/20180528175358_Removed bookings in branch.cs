using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class Removedbookingsinbranch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Branches_BranchId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyBranch_Companies_fk_Booking",
                table: "CompanyBranch");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyTag_Companies_fk_Booking",
                table: "CompanyTag");

            migrationBuilder.DropIndex(
                name: "IX_CompanyTag_fk_Booking",
                table: "CompanyTag");

            migrationBuilder.DropIndex(
                name: "IX_CompanyBranch_fk_Booking",
                table: "CompanyBranch");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_BranchId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "fk_Booking",
                table: "CompanyTag");

            migrationBuilder.DropColumn(
                name: "fk_Booking",
                table: "CompanyBranch");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyTag_fk_Company",
                table: "CompanyTag",
                column: "fk_Company");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyBranch_fk_Company",
                table: "CompanyBranch",
                column: "fk_Company");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyBranch_Companies_fk_Company",
                table: "CompanyBranch",
                column: "fk_Company",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyTag_Companies_fk_Company",
                table: "CompanyTag",
                column: "fk_Company",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyBranch_Companies_fk_Company",
                table: "CompanyBranch");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyTag_Companies_fk_Company",
                table: "CompanyTag");

            migrationBuilder.DropIndex(
                name: "IX_CompanyTag_fk_Company",
                table: "CompanyTag");

            migrationBuilder.DropIndex(
                name: "IX_CompanyBranch_fk_Company",
                table: "CompanyBranch");

            migrationBuilder.AddColumn<int>(
                name: "fk_Booking",
                table: "CompanyTag",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "fk_Booking",
                table: "CompanyBranch",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Bookings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyTag_fk_Booking",
                table: "CompanyTag",
                column: "fk_Booking");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyBranch_fk_Booking",
                table: "CompanyBranch",
                column: "fk_Booking");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_BranchId",
                table: "Bookings",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Branches_BranchId",
                table: "Bookings",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyBranch_Companies_fk_Booking",
                table: "CompanyBranch",
                column: "fk_Booking",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyTag_Companies_fk_Booking",
                table: "CompanyTag",
                column: "fk_Booking",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
