using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Presentations_PresentationId",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Branches_PresentationId",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "PresentationId",
                table: "Branches");

            migrationBuilder.CreateTable(
                name: "PresentationBranches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    fk_Branch = table.Column<int>(nullable: false),
                    fk_Presentation = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresentationBranches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PresentationBranches_Branches_fk_Branch",
                        column: x => x.fk_Branch,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PresentationBranches_Presentations_fk_Presentation",
                        column: x => x.fk_Presentation,
                        principalTable: "Presentations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PresentationBranches_fk_Branch",
                table: "PresentationBranches",
                column: "fk_Branch");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationBranches_fk_Presentation",
                table: "PresentationBranches",
                column: "fk_Presentation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PresentationBranches");

            migrationBuilder.AddColumn<int>(
                name: "PresentationId",
                table: "Branches",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Branches_PresentationId",
                table: "Branches",
                column: "PresentationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_Presentations_PresentationId",
                table: "Branches",
                column: "PresentationId",
                principalTable: "Presentations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
