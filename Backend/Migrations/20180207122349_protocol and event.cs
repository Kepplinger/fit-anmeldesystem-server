using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class protocolandevent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Areas_Events_FK_Areas",
                table: "Areas");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Areas_FK_Locations",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "FK_Areas",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "FK_Locations",
                table: "Locations",
                newName: "AreaId");

            migrationBuilder.RenameIndex(
                name: "IX_Locations_FK_Locations",
                table: "Locations",
                newName: "IX_Locations_AreaId");

            migrationBuilder.RenameColumn(
                name: "FK_Areas",
                table: "Areas",
                newName: "EventId");

            migrationBuilder.RenameIndex(
                name: "IX_Areas_FK_Areas",
                table: "Areas",
                newName: "IX_Areas_EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_Events_EventId",
                table: "Areas",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Areas_AreaId",
                table: "Locations",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Areas_Events_EventId",
                table: "Areas");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Areas_AreaId",
                table: "Locations");

            migrationBuilder.RenameColumn(
                name: "AreaId",
                table: "Locations",
                newName: "FK_Locations");

            migrationBuilder.RenameIndex(
                name: "IX_Locations_AreaId",
                table: "Locations",
                newName: "IX_Locations_FK_Locations");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "Areas",
                newName: "FK_Areas");

            migrationBuilder.RenameIndex(
                name: "IX_Areas_EventId",
                table: "Areas",
                newName: "IX_Areas_FK_Areas");

            migrationBuilder.AddColumn<int>(
                name: "FK_Areas",
                table: "Events",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_Events_FK_Areas",
                table: "Areas",
                column: "FK_Areas",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Areas_FK_Locations",
                table: "Locations",
                column: "FK_Locations",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
