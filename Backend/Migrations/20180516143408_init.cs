using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_Branches_Bookings_BookingId",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "fk_Branches_Presentations_fk_Presentation",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Branches_BookingId",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "Branches");

            migrationBuilder.RenameColumn(
                name: "fk_Presentation",
                table: "Branches",
                newName: "PresentationId");

            migrationBuilder.RenameIndex(
                name: "IX_Branches_fk_Presentation",
                table: "Branches",
                newName: "IX_Branches_PresentationId");

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Bookings",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BookingBranches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    fk_Booking = table.Column<int>(nullable: true),
                    fk_Branch = table.Column<int>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingBranches", x => x.Id);
                    table.ForeignKey(
                        name: "fk_BookingBranches_Bookings_fk_Booking",
                        column: x => x.fk_Booking,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_BookingBranches_Branches_fk_Branch",
                        column: x => x.fk_Branch,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_BranchId",
                table: "Bookings",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingBranches_fk_Booking",
                table: "BookingBranches",
                column: "fk_Booking");

            migrationBuilder.CreateIndex(
                name: "IX_BookingBranches_fk_Branch",
                table: "BookingBranches",
                column: "fk_Branch");

            migrationBuilder.AddForeignKey(
                name: "fk_Bookings_Branches_BranchId",
                table: "Bookings",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_Branches_Presentations_PresentationId",
                table: "Branches",
                column: "PresentationId",
                principalTable: "Presentations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_Bookings_Branches_BranchId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "fk_Branches_Presentations_PresentationId",
                table: "Branches");

            migrationBuilder.DropTable(
                name: "BookingBranches");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_BranchId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "PresentationId",
                table: "Branches",
                newName: "fk_Presentation");

            migrationBuilder.RenameIndex(
                name: "IX_Branches_PresentationId",
                table: "Branches",
                newName: "IX_Branches_fk_Presentation");

            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "Branches",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Branches_BookingId",
                table: "Branches",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "fk_Branches_Bookings_BookingId",
                table: "Branches",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_Branches_Presentations_fk_Presentation",
                table: "Branches",
                column: "fk_Presentation",
                principalTable: "Presentations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
