using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class final14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Packages_FK_Package",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_FK_Package",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "FK_Package",
                table: "Bookings");

            migrationBuilder.AddColumn<int>(
                name: "FK_FitPackage",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ChangeProtocols",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChangeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ColumName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeProtocols", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_FK_FitPackage",
                table: "Bookings",
                column: "FK_FitPackage");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Packages_FK_FitPackage",
                table: "Bookings",
                column: "FK_FitPackage",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Packages_FK_FitPackage",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "ChangeProtocols");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_FK_FitPackage",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "FK_FitPackage",
                table: "Bookings");

            migrationBuilder.AddColumn<int>(
                name: "FK_Package",
                table: "Bookings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_FK_Package",
                table: "Bookings",
                column: "FK_Package");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Packages_FK_Package",
                table: "Bookings",
                column: "FK_Package",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
