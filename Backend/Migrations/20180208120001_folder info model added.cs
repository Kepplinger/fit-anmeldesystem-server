using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class folderinfomodeladded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Branch",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "EstablishmentsAut",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "EstablishmentsCountAut",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "EstablishmentsInt",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Homepage",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "EstablishmentsCountInt",
                table: "Companies",
                newName: "FK_FolderInfo");

            migrationBuilder.AddColumn<bool>(
                name: "IsPending",
                table: "Companies",
                nullable: false,
                defaultValue: false);

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
                    Logo = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolderInfos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_FK_FolderInfo",
                table: "Companies",
                column: "FK_FolderInfo");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_FolderInfos_FK_FolderInfo",
                table: "Companies",
                column: "FK_FolderInfo",
                principalTable: "FolderInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_FolderInfos_FK_FolderInfo",
                table: "Companies");

            migrationBuilder.DropTable(
                name: "FolderInfos");

            migrationBuilder.DropIndex(
                name: "IX_Companies_FK_FolderInfo",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "IsPending",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "FK_FolderInfo",
                table: "Companies",
                newName: "EstablishmentsCountInt");

            migrationBuilder.AddColumn<string>(
                name: "Branch",
                table: "Companies",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Companies",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EstablishmentsAut",
                table: "Companies",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EstablishmentsCountAut",
                table: "Companies",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EstablishmentsInt",
                table: "Companies",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Homepage",
                table: "Companies",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "Companies",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Companies",
                nullable: false,
                defaultValue: "");
        }
    }
}
