using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class changeidandcolumnnameedit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ColumName",
                table: "ChangeProtocols",
                newName: "ColumnName");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "ChangeProtocols",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsPending",
                table: "ChangeProtocols",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RowId",
                table: "ChangeProtocols",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "ChangeProtocols");

            migrationBuilder.DropColumn(
                name: "IsPending",
                table: "ChangeProtocols");

            migrationBuilder.DropColumn(
                name: "RowId",
                table: "ChangeProtocols");

            migrationBuilder.RenameColumn(
                name: "ColumnName",
                table: "ChangeProtocols",
                newName: "ColumName");
        }
    }
}
