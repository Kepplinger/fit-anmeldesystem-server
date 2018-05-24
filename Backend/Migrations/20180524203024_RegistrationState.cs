using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class RegistrationState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCurrent",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Events");

            migrationBuilder.AddColumn<int>(
                name: "RegistrationStateId",
                table: "Events",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RegistrationState",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsCurrent = table.Column<bool>(nullable: false),
                    IsLocked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationState", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_RegistrationStateId",
                table: "Events",
                column: "RegistrationStateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_RegistrationState_RegistrationStateId",
                table: "Events",
                column: "RegistrationStateId",
                principalTable: "RegistrationState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_RegistrationState_RegistrationStateId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "RegistrationState");

            migrationBuilder.DropIndex(
                name: "IX_Events_RegistrationStateId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "RegistrationStateId",
                table: "Events");

            migrationBuilder.AddColumn<bool>(
                name: "IsCurrent",
                table: "Events",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "Events",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte[]>(
                name: "Timestamp",
                table: "Events",
                rowVersion: true,
                nullable: true);
        }
    }
}
