﻿using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Addition = table.Column<string>(nullable: true),
                    City = table.Column<string>(maxLength: 25, nullable: false),
                    Street = table.Column<string>(maxLength: 50, nullable: false),
                    StreetNumber = table.Column<string>(maxLength: 10, nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    ZipCode = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChangeProtocols",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChangeDate = table.Column<DateTime>(nullable: false),
                    ColumnName = table.Column<string>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    IsPending = table.Column<bool>(nullable: false),
                    NewValue = table.Column<string>(nullable: true),
                    OldValue = table.Column<string>(nullable: true),
                    RowId = table.Column<int>(nullable: false),
                    TableName = table.Column<string>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    isAdminChange = table.Column<bool>(nullable: false),
                    isReverted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeProtocols", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 15, nullable: false),
                    Gender = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(maxLength: 15, nullable: false),
                    PhoneNumber = table.Column<string>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Emails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 25, nullable: false),
                    Subject = table.Column<string>(nullable: false),
                    Template = table.Column<string>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EventDate = table.Column<DateTime>(nullable: false),
                    IsCurrent = table.Column<bool>(nullable: false),
                    IsLocked = table.Column<bool>(nullable: false),
                    RegistrationEnd = table.Column<DateTime>(nullable: false),
                    RegistrationStart = table.Column<DateTime>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Discriminator = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Presentations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    FileURL = table.Column<string>(nullable: true),
                    IsAccepted = table.Column<bool>(nullable: false),
                    RoomNumber = table.Column<string>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Title = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Presentations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Graduates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: true),
                    fk_Address = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 30, nullable: false),
                    Gender = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(maxLength: 30, nullable: false),
                    PhoneNumber = table.Column<string>(nullable: false),
                    RegistrationToken = table.Column<string>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Graduates", x => x.Id);
                    table.ForeignKey(
                        name: "fk_Graduates_Addresses_fk_Address",
                        column: x => x.fk_Address,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "fk_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "fk_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "fk_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "fk_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "fk_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    fk_Address = table.Column<int>(nullable: false),
                    fk_Contact = table.Column<int>(nullable: false),
                    IsPending = table.Column<bool>(nullable: false),
                    MemberPaymentAmount = table.Column<double>(nullable: false),
                    MemberSince = table.Column<int>(nullable: false),
                    MemberStatus = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    RegistrationToken = table.Column<string>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "fk_Companies_Addresses_fk_Address",
                        column: x => x.fk_Address,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_Companies_Contacts_fk_Contact",
                        column: x => x.fk_Contact,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Designation = table.Column<string>(nullable: false),
                    fk_Event = table.Column<int>(nullable: true),
                    GraphicUrl = table.Column<string>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                    table.ForeignKey(
                        name: "fk_Areas_Events_fk_Event",
                        column: x => x.fk_Event,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<int>(nullable: true),
                    IsArchive = table.Column<bool>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                    table.ForeignKey(
                        name: "fk_Tag_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AreaId = table.Column<int>(nullable: true),
                    Category = table.Column<string>(nullable: false),
                    Number = table.Column<string>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    XCoordinate = table.Column<double>(nullable: false),
                    YCoordinate = table.Column<double>(nullable: false),
                    isOccupied = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "fk_Locations_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdditionalInfo = table.Column<string>(maxLength: 500, nullable: true),
                    Branch = table.Column<string>(maxLength: 20, nullable: false),
                    CompanyDescription = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    EstablishmentsAut = table.Column<string>(maxLength: 30, nullable: true),
                    EstablishmentsCountAut = table.Column<int>(nullable: false),
                    EstablishmentsCountInt = table.Column<int>(nullable: false),
                    EstablishmentsInt = table.Column<string>(maxLength: 30, nullable: true),
                    fk_Company = table.Column<int>(nullable: false),
                    fk_Contact = table.Column<int>(nullable: true),
                    fk_Event = table.Column<int>(nullable: false),
                    fk_FitPackage = table.Column<int>(nullable: false),
                    fk_Location = table.Column<int>(nullable: true),
                    fk_Presentation = table.Column<int>(nullable: true),
                    Homepage = table.Column<string>(nullable: false),
                    Logo = table.Column<string>(nullable: true),
                    PdfFilePath = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: false),
                    ProvidesSummerJob = table.Column<bool>(nullable: false),
                    ProvidesThesis = table.Column<bool>(nullable: false),
                    Remarks = table.Column<string>(maxLength: 500, nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    isAccepted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "fk_Bookings_Companies_fk_Company",
                        column: x => x.fk_Company,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_Bookings_Contacts_fk_Contact",
                        column: x => x.fk_Contact,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_Bookings_Events_fk_Event",
                        column: x => x.fk_Event,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_Bookings_Packages_fk_FitPackage",
                        column: x => x.fk_FitPackage,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_Bookings_Locations_fk_Location",
                        column: x => x.fk_Location,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_Bookings_Presentations_fk_Presentation",
                        column: x => x.fk_Presentation,
                        principalTable: "Presentations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BookingId = table.Column<int>(nullable: true),
                    fk_Presentation = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                    table.ForeignKey(
                        name: "fk_Branches_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_Branches_Presentations_fk_Presentation",
                        column: x => x.fk_Presentation,
                        principalTable: "Presentations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rerpresentatives",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BookingId = table.Column<int>(nullable: true),
                    Email = table.Column<string>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rerpresentatives", x => x.Id);
                    table.ForeignKey(
                        name: "fk_Rerpresentatives_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResourceBookings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    fk_Booking = table.Column<int>(nullable: true),
                    fk_Resource = table.Column<int>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceBookings", x => x.Id);
                    table.ForeignKey(
                        name: "fk_ResourceBookings_Bookings_fk_Booking",
                        column: x => x.fk_Booking,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_ResourceBookings_Resources_fk_Resource",
                        column: x => x.fk_Resource,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Areas_fk_Event",
                table: "Areas",
                column: "fk_Event");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_fk_Company",
                table: "Bookings",
                column: "fk_Company");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_fk_Contact",
                table: "Bookings",
                column: "fk_Contact");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_fk_Event",
                table: "Bookings",
                column: "fk_Event");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_fk_FitPackage",
                table: "Bookings",
                column: "fk_FitPackage");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_fk_Location",
                table: "Bookings",
                column: "fk_Location");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_fk_Presentation",
                table: "Bookings",
                column: "fk_Presentation");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_BookingId",
                table: "Branches",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_fk_Presentation",
                table: "Branches",
                column: "fk_Presentation");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_fk_Address",
                table: "Companies",
                column: "fk_Address");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_fk_Contact",
                table: "Companies",
                column: "fk_Contact");

            migrationBuilder.CreateIndex(
                name: "IX_Graduates_fk_Address",
                table: "Graduates",
                column: "fk_Address");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_AreaId",
                table: "Locations",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Rerpresentatives_BookingId",
                table: "Rerpresentatives",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceBookings_fk_Booking",
                table: "ResourceBookings",
                column: "fk_Booking");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceBookings_fk_Resource",
                table: "ResourceBookings",
                column: "fk_Resource");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_CompanyId",
                table: "Tag",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "ChangeProtocols");

            migrationBuilder.DropTable(
                name: "Emails");

            migrationBuilder.DropTable(
                name: "Graduates");

            migrationBuilder.DropTable(
                name: "Rerpresentatives");

            migrationBuilder.DropTable(
                name: "ResourceBookings");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Presentations");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
