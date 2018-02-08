using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class fkfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_FolderInfos_FK_FolderInfo",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_FK_FolderInfo",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "FK_FolderInfo",
                table: "Companies");

            migrationBuilder.AddColumn<int>(
                name: "FolderInfoId",
                table: "Companies",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_FolderInfos_FolderInfoId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_FolderInfoId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "FolderInfoId",
                table: "Companies");

            migrationBuilder.AddColumn<int>(
                name: "FK_FolderInfo",
                table: "Companies",
                nullable: false,
                defaultValue: 0);

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
    }
}
