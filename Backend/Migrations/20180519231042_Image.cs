using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class Image : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Rerpresentatives");

            migrationBuilder.DropColumn(
                name: "FileURL",
                table: "Presentations");

            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "GraphicUrl",
                table: "Areas");

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Rerpresentatives",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "Presentations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LogoId",
                table: "Bookings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GraphicId",
                table: "Areas",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DataFile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DataUrl = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataFile", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rerpresentatives_ImageId",
                table: "Rerpresentatives",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Presentations_FileId",
                table: "Presentations",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_LogoId",
                table: "Bookings",
                column: "LogoId");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_GraphicId",
                table: "Areas",
                column: "GraphicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_DataFile_GraphicId",
                table: "Areas",
                column: "GraphicId",
                principalTable: "DataFile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_DataFile_LogoId",
                table: "Bookings",
                column: "LogoId",
                principalTable: "DataFile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Presentations_DataFile_FileId",
                table: "Presentations",
                column: "FileId",
                principalTable: "DataFile",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Areas_DataFile_GraphicId",
                table: "Areas");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_DataFile_LogoId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Presentations_DataFile_FileId",
                table: "Presentations");

            migrationBuilder.DropForeignKey(
                name: "FK_Rerpresentatives_DataFile_ImageId",
                table: "Rerpresentatives");

            migrationBuilder.DropTable(
                name: "DataFile");

            migrationBuilder.DropIndex(
                name: "IX_Rerpresentatives_ImageId",
                table: "Rerpresentatives");

            migrationBuilder.DropIndex(
                name: "IX_Presentations_FileId",
                table: "Presentations");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_LogoId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Areas_GraphicId",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Rerpresentatives");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Presentations");

            migrationBuilder.DropColumn(
                name: "LogoId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "GraphicId",
                table: "Areas");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Rerpresentatives",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileURL",
                table: "Presentations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "Bookings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GraphicUrl",
                table: "Areas",
                nullable: true);
        }
    }
}
