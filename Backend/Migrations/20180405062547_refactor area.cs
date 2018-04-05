﻿using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class refactorarea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Areas_Events_EventId",
                table: "Areas");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "Areas",
                newName: "FK_Event");

            migrationBuilder.RenameIndex(
                name: "IX_Areas_EventId",
                table: "Areas",
                newName: "IX_Areas_FK_Event");

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_Events_FK_Event",
                table: "Areas",
                column: "FK_Event",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Areas_Events_FK_Event",
                table: "Areas");

            migrationBuilder.RenameColumn(
                name: "FK_Event",
                table: "Areas",
                newName: "EventId");

            migrationBuilder.RenameIndex(
                name: "IX_Areas_FK_Event",
                table: "Areas",
                newName: "IX_Areas_EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_Events_EventId",
                table: "Areas",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}