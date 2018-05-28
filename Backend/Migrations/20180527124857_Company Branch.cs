using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class CompanyBranch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyBranch",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    fk_Booking = table.Column<int>(nullable: true),
                    fk_Branch = table.Column<int>(nullable: false),
                    fk_Company = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyBranch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyBranch_Companies_fk_Booking",
                        column: x => x.fk_Booking,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyBranch_Branches_fk_Branch",
                        column: x => x.fk_Branch,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyBranch_fk_Booking",
                table: "CompanyBranch",
                column: "fk_Booking");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyBranch_fk_Branch",
                table: "CompanyBranch",
                column: "fk_Branch");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyBranch");
        }
    }
}
