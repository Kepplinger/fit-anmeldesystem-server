using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class addedemailtemplates : Migration
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

            migrationBuilder.CreateTable(
                name: "Emails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 300, nullable: false),
                    Name = table.Column<string>(maxLength: 25, nullable: false),
                    Subject = table.Column<string>(nullable: false),
                    Template = table.Column<string>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emails", x => x.Id);
                });

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

            migrationBuilder.DropTable(
                name: "Emails");

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
