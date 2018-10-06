using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class Yeah : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_MemberStatus_fk_MemberStatus",
                table: "Companies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberStatus",
                table: "MemberStatus");

            migrationBuilder.RenameTable(
                name: "MemberStatus",
                newName: "MemberStati");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberStati",
                table: "MemberStati",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_MemberStati_fk_MemberStatus",
                table: "Companies",
                column: "fk_MemberStatus",
                principalTable: "MemberStati",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_MemberStati_fk_MemberStatus",
                table: "Companies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberStati",
                table: "MemberStati");

            migrationBuilder.RenameTable(
                name: "MemberStati",
                newName: "MemberStatus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberStatus",
                table: "MemberStatus",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_MemberStatus_fk_MemberStatus",
                table: "Companies",
                column: "fk_MemberStatus",
                principalTable: "MemberStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
