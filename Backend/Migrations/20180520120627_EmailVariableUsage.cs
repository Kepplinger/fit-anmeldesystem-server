using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class EmailVariableUsage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailVariable_Emails_EmailId",
                table: "EmailVariable");

            migrationBuilder.DropIndex(
                name: "IX_EmailVariable_EmailId",
                table: "EmailVariable");

            migrationBuilder.DropColumn(
                name: "EmailId",
                table: "EmailVariable");

            migrationBuilder.CreateTable(
                name: "EmailVariableUsage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    fk_Email = table.Column<int>(nullable: true),
                    fk_EmailVariable = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailVariableUsage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailVariableUsage_Emails_fk_Email",
                        column: x => x.fk_Email,
                        principalTable: "Emails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmailVariableUsage_EmailVariable_fk_EmailVariable",
                        column: x => x.fk_EmailVariable,
                        principalTable: "EmailVariable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailVariableUsage_fk_Email",
                table: "EmailVariableUsage",
                column: "fk_Email");

            migrationBuilder.CreateIndex(
                name: "IX_EmailVariableUsage_fk_EmailVariable",
                table: "EmailVariableUsage",
                column: "fk_EmailVariable");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailVariableUsage");

            migrationBuilder.AddColumn<int>(
                name: "EmailId",
                table: "EmailVariable",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailVariable_EmailId",
                table: "EmailVariable",
                column: "EmailId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailVariable_Emails_EmailId",
                table: "EmailVariable",
                column: "EmailId",
                principalTable: "Emails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
