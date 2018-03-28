using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class refactoredFolderInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_FolderInfos_FolderInfoId",
                table: "Companies");

            migrationBuilder.DropTable(
                name: "FolderInfos");

            migrationBuilder.DropIndex(
                name: "IX_Companies_FolderInfoId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "FolderInfoId",
                table: "Companies");

            migrationBuilder.AddColumn<string>(
                name: "Branch",
                table: "Bookings",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Bookings",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EstablishmentsAut",
                table: "Bookings",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EstablishmentsCountAut",
                table: "Bookings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EstablishmentsCountInt",
                table: "Bookings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EstablishmentsInt",
                table: "Bookings",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Homepage",
                table: "Bookings",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "Bookings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Bookings",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Branch",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "EstablishmentsAut",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "EstablishmentsCountAut",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "EstablishmentsCountInt",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "EstablishmentsInt",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Homepage",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Bookings");

            migrationBuilder.AddColumn<int>(
                name: "FolderInfoId",
                table: "Companies",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FolderInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Branch = table.Column<string>(maxLength: 20, nullable: false),
                    Email = table.Column<string>(nullable: false),
                    EstablishmentsAut = table.Column<string>(maxLength: 30, nullable: true),
                    EstablishmentsCountAut = table.Column<int>(nullable: false),
                    EstablishmentsCountInt = table.Column<int>(nullable: false),
                    EstablishmentsInt = table.Column<string>(maxLength: 30, nullable: true),
                    Homepage = table.Column<string>(nullable: false),
                    Logo = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolderInfos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_FolderInfoId",
                table: "Companies",
                column: "FolderInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_FolderInfos_FolderInfoId",
                table: "Companies",
                column: "FolderInfoId",
                principalTable: "FolderInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
