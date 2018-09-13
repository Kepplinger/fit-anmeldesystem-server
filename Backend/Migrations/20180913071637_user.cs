using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rerpresentatives_Bookings_BookingId",
                table: "Rerpresentatives");

            migrationBuilder.DropForeignKey(
                name: "FK_Rerpresentatives_DataFile_ImageId",
                table: "Rerpresentatives");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rerpresentatives",
                table: "Rerpresentatives");

            migrationBuilder.RenameTable(
                name: "Rerpresentatives",
                newName: "Representatives");

            migrationBuilder.RenameIndex(
                name: "IX_Rerpresentatives_ImageId",
                table: "Representatives",
                newName: "IX_Representatives_ImageId");

            migrationBuilder.RenameIndex(
                name: "IX_Rerpresentatives_BookingId",
                table: "Representatives",
                newName: "IX_Representatives_BookingId");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Representatives",
                table: "Representatives",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Representatives_Bookings_BookingId",
                table: "Representatives",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Representatives_DataFile_ImageId",
                table: "Representatives",
                column: "ImageId",
                principalTable: "DataFile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Representatives_Bookings_BookingId",
                table: "Representatives");

            migrationBuilder.DropForeignKey(
                name: "FK_Representatives_DataFile_ImageId",
                table: "Representatives");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Representatives",
                table: "Representatives");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "Representatives",
                newName: "Rerpresentatives");

            migrationBuilder.RenameIndex(
                name: "IX_Representatives_ImageId",
                table: "Rerpresentatives",
                newName: "IX_Rerpresentatives_ImageId");

            migrationBuilder.RenameIndex(
                name: "IX_Representatives_BookingId",
                table: "Rerpresentatives",
                newName: "IX_Rerpresentatives_BookingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rerpresentatives",
                table: "Rerpresentatives",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rerpresentatives_Bookings_BookingId",
                table: "Rerpresentatives",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rerpresentatives_DataFile_ImageId",
                table: "Rerpresentatives",
                column: "ImageId",
                principalTable: "DataFile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
