using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class MembeSatu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemberSince",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "MemberStatus",
                table: "Companies",
                newName: "fk_MemberStatus");

            migrationBuilder.CreateTable(
                name: "MemberStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DefaultPrice = table.Column<double>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberStatus", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_fk_MemberStatus",
                table: "Companies",
                column: "fk_MemberStatus");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_MemberStatus_fk_MemberStatus",
                table: "Companies",
                column: "fk_MemberStatus",
                principalTable: "MemberStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_MemberStatus_fk_MemberStatus",
                table: "Companies");

            migrationBuilder.DropTable(
                name: "MemberStatus");

            migrationBuilder.DropIndex(
                name: "IX_Companies_fk_MemberStatus",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "fk_MemberStatus",
                table: "Companies",
                newName: "MemberStatus");

            migrationBuilder.AddColumn<int>(
                name: "MemberSince",
                table: "Companies",
                nullable: false,
                defaultValue: 0);
        }
    }
}
