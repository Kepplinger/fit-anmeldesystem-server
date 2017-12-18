using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class someeditingdidforjsonserial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "Zip",
                table: "Addresses");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Branch",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EstablishmentsAut",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EstablishmentsCountAut",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EstablishmentsCountInt",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EstablishmentsInt",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "Addresses",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "Branch",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "EstablishmentsAut",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "EstablishmentsCountAut",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "EstablishmentsCountInt",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "EstablishmentsInt",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "Addresses");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Contacts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zip",
                table: "Addresses",
                nullable: false,
                defaultValue: "");
        }
    }
}
