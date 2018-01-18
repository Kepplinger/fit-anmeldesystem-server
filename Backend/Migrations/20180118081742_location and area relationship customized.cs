using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class locationandarearelationshipcustomized : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Areas_FK_Area",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_FK_Area",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "FK_Area",
                table: "Locations");

            migrationBuilder.RenameColumn(
                name: "Graphic",
                table: "Areas",
                newName: "GraphicURL");

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Locations",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "FK_Locations",
                table: "Locations",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isOccupied",
                table: "Locations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_FK_Locations",
                table: "Locations",
                column: "FK_Locations");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Areas_FK_Locations",
                table: "Locations",
                column: "FK_Locations",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Areas_FK_Locations",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_FK_Locations",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "FK_Locations",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "isOccupied",
                table: "Locations");

            migrationBuilder.RenameColumn(
                name: "GraphicURL",
                table: "Areas",
                newName: "Graphic");

            migrationBuilder.AlterColumn<int>(
                name: "Number",
                table: "Locations",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "FK_Area",
                table: "Locations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_FK_Area",
                table: "Locations",
                column: "FK_Area");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Areas_FK_Area",
                table: "Locations",
                column: "FK_Area",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
