using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class CompanyTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Companies_CompanyId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_CompanyId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Tags");

            migrationBuilder.CreateTable(
                name: "CompanyTag",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    fk_Booking = table.Column<int>(nullable: true),
                    fk_Company = table.Column<int>(nullable: true),
                    fk_Tag = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyTag_Companies_fk_Booking",
                        column: x => x.fk_Booking,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyTag_Tags_fk_Tag",
                        column: x => x.fk_Tag,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyTag_fk_Booking",
                table: "CompanyTag",
                column: "fk_Booking");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyTag_fk_Tag",
                table: "CompanyTag",
                column: "fk_Tag");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyTag");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Tags",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_CompanyId",
                table: "Tags",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Companies_CompanyId",
                table: "Tags",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
