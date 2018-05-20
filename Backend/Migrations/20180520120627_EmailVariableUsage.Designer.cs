﻿// <auto-generated />
using Backend.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Backend.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180520120627_EmailVariableUsage")]
    partial class EmailVariableUsage
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Backend.Core.EmailVariable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Entity")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("Value")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("EmailVariable");
                });

            modelBuilder.Entity("Backend.Core.Entities.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Addition");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("StreetNumber")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Backend.Core.Entities.Area", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Designation")
                        .IsRequired();

                    b.Property<int?>("GraphicId");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int?>("fk_Event");

                    b.HasKey("Id");

                    b.HasIndex("GraphicId");

                    b.HasIndex("fk_Event");

                    b.ToTable("Areas");
                });

            modelBuilder.Entity("Backend.Core.Entities.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AdditionalInfo")
                        .HasMaxLength(500);

                    b.Property<string>("Branch")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<int?>("BranchId");

                    b.Property<string>("CompanyDescription");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("EstablishmentsAut")
                        .HasMaxLength(30);

                    b.Property<int>("EstablishmentsCountAut");

                    b.Property<int>("EstablishmentsCountInt");

                    b.Property<string>("EstablishmentsInt")
                        .HasMaxLength(30);

                    b.Property<string>("Homepage")
                        .IsRequired();

                    b.Property<int?>("LogoId");

                    b.Property<string>("PdfFilePath");

                    b.Property<string>("PhoneNumber")
                        .IsRequired();

                    b.Property<bool>("ProvidesSummerJob");

                    b.Property<bool>("ProvidesThesis");

                    b.Property<string>("Remarks")
                        .HasMaxLength(500);

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int>("fk_Company");

                    b.Property<int?>("fk_Contact");

                    b.Property<int>("fk_Event");

                    b.Property<int>("fk_FitPackage");

                    b.Property<int?>("fk_Location");

                    b.Property<int?>("fk_Presentation");

                    b.Property<bool>("isAccepted");

                    b.HasKey("Id");

                    b.HasIndex("BranchId");

                    b.HasIndex("LogoId");

                    b.HasIndex("fk_Company");

                    b.HasIndex("fk_Contact");

                    b.HasIndex("fk_Event");

                    b.HasIndex("fk_FitPackage");

                    b.HasIndex("fk_Location");

                    b.HasIndex("fk_Presentation");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("Backend.Core.Entities.BookingBranches", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int?>("fk_Booking");

                    b.Property<int>("fk_Branch");

                    b.HasKey("Id");

                    b.HasIndex("fk_Booking");

                    b.HasIndex("fk_Branch");

                    b.ToTable("BookingBranches");
                });

            modelBuilder.Entity("Backend.Core.Entities.Branch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("Branches");
                });

            modelBuilder.Entity("Backend.Core.Entities.ChangeProtocol", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ChangeDate");

                    b.Property<string>("ColumnName");

                    b.Property<int>("CompanyId");

                    b.Property<bool>("IsPending");

                    b.Property<string>("NewValue");

                    b.Property<string>("OldValue");

                    b.Property<int>("RowId");

                    b.Property<string>("TableName");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("isAdminChange");

                    b.Property<bool>("isReverted");

                    b.HasKey("Id");

                    b.ToTable("ChangeProtocols");
                });

            modelBuilder.Entity("Backend.Core.Entities.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsPending");

                    b.Property<double>("MemberPaymentAmount");

                    b.Property<int>("MemberSince");

                    b.Property<int>("MemberStatus");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("RegistrationToken")
                        .IsRequired();

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int>("fk_Address");

                    b.Property<int>("fk_Contact");

                    b.HasKey("Id");

                    b.HasIndex("fk_Address");

                    b.HasIndex("fk_Contact");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Backend.Core.Entities.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(15);

                    b.Property<string>("Gender")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(15);

                    b.Property<string>("PhoneNumber")
                        .IsRequired();

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("Backend.Core.Entities.DataFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DataUrl");

                    b.Property<string>("Name");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("DataFile");
                });

            modelBuilder.Entity("Backend.Core.Entities.Email", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.Property<string>("Subject")
                        .IsRequired();

                    b.Property<string>("Template")
                        .IsRequired();

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("Emails");
                });

            modelBuilder.Entity("Backend.Core.Entities.EmailVariableUsage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int?>("fk_Email");

                    b.Property<int>("fk_EmailVariable");

                    b.HasKey("Id");

                    b.HasIndex("fk_Email");

                    b.HasIndex("fk_EmailVariable");

                    b.ToTable("EmailVariableUsage");
                });

            modelBuilder.Entity("Backend.Core.Entities.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EventDate");

                    b.Property<bool>("IsCurrent");

                    b.Property<bool>("IsLocked");

                    b.Property<DateTime>("RegistrationEnd");

                    b.Property<DateTime>("RegistrationStart");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Backend.Core.Entities.FitPackage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int>("Discriminator");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Price");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("Packages");
                });

            modelBuilder.Entity("Backend.Core.Entities.Graduate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("Gender")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("PhoneNumber")
                        .IsRequired();

                    b.Property<string>("RegistrationToken")
                        .IsRequired();

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int>("fk_Address");

                    b.HasKey("Id");

                    b.HasIndex("fk_Address");

                    b.ToTable("Graduates");
                });

            modelBuilder.Entity("Backend.Core.Entities.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AreaId");

                    b.Property<string>("Category")
                        .IsRequired();

                    b.Property<string>("Number")
                        .IsRequired();

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<double>("XCoordinate");

                    b.Property<double>("YCoordinate");

                    b.Property<bool>("isOccupied");

                    b.HasKey("Id");

                    b.HasIndex("AreaId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Backend.Core.Entities.Presentation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasMaxLength(500);

                    b.Property<int?>("FileId");

                    b.Property<bool>("IsAccepted");

                    b.Property<string>("RoomNumber");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.HasIndex("FileId");

                    b.ToTable("Presentations");
                });

            modelBuilder.Entity("Backend.Core.Entities.PresentationBranches", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int>("fk_Branch");

                    b.Property<int?>("fk_Presentation");

                    b.HasKey("Id");

                    b.HasIndex("fk_Branch");

                    b.HasIndex("fk_Presentation");

                    b.ToTable("PresentationBranches");
                });

            modelBuilder.Entity("Backend.Core.Entities.Representative", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("BookingId");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<int?>("ImageId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("BookingId");

                    b.HasIndex("ImageId");

                    b.ToTable("Rerpresentatives");
                });

            modelBuilder.Entity("Backend.Core.Entities.Resource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("Backend.Core.Entities.ResourceBooking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int?>("fk_Booking");

                    b.Property<int>("fk_Resource");

                    b.HasKey("Id");

                    b.HasIndex("fk_Booking");

                    b.HasIndex("fk_Resource");

                    b.ToTable("ResourceBookings");
                });

            modelBuilder.Entity("Backend.Core.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CompanyId");

                    b.Property<bool>("IsArchive");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Backend.Core.Entities.Area", b =>
                {
                    b.HasOne("Backend.Core.Entities.DataFile", "Graphic")
                        .WithMany()
                        .HasForeignKey("GraphicId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Core.Entities.Event", "Event")
                        .WithMany("Areas")
                        .HasForeignKey("fk_Event")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Core.Entities.Booking", b =>
                {
                    b.HasOne("Backend.Core.Entities.Branch")
                        .WithMany("Bookings")
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Core.Entities.DataFile", "Logo")
                        .WithMany()
                        .HasForeignKey("LogoId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Core.Entities.Company", "Company")
                        .WithMany()
                        .HasForeignKey("fk_Company")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Core.Entities.Contact", "Contact")
                        .WithMany()
                        .HasForeignKey("fk_Contact")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Core.Entities.Event", "Event")
                        .WithMany()
                        .HasForeignKey("fk_Event")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Core.Entities.FitPackage", "FitPackage")
                        .WithMany()
                        .HasForeignKey("fk_FitPackage")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Core.Entities.Location", "Location")
                        .WithMany()
                        .HasForeignKey("fk_Location")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Core.Entities.Presentation", "Presentation")
                        .WithMany()
                        .HasForeignKey("fk_Presentation")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Core.Entities.BookingBranches", b =>
                {
                    b.HasOne("Backend.Core.Entities.Booking", "Booking")
                        .WithMany("Branches")
                        .HasForeignKey("fk_Booking")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Core.Entities.Branch", "Branch")
                        .WithMany()
                        .HasForeignKey("fk_Branch")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Core.Entities.Company", b =>
                {
                    b.HasOne("Backend.Core.Entities.Address", "Address")
                        .WithMany()
                        .HasForeignKey("fk_Address")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Core.Entities.Contact", "Contact")
                        .WithMany()
                        .HasForeignKey("fk_Contact")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Core.Entities.EmailVariableUsage", b =>
                {
                    b.HasOne("Backend.Core.Entities.Email", "Email")
                        .WithMany("AvailableVariables")
                        .HasForeignKey("fk_Email")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Core.EmailVariable", "EmailVariable")
                        .WithMany()
                        .HasForeignKey("fk_EmailVariable")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Core.Entities.Graduate", b =>
                {
                    b.HasOne("Backend.Core.Entities.Address", "Address")
                        .WithMany()
                        .HasForeignKey("fk_Address")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Core.Entities.Location", b =>
                {
                    b.HasOne("Backend.Core.Entities.Area")
                        .WithMany("Locations")
                        .HasForeignKey("AreaId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Core.Entities.Presentation", b =>
                {
                    b.HasOne("Backend.Core.Entities.DataFile", "File")
                        .WithMany()
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Core.Entities.PresentationBranches", b =>
                {
                    b.HasOne("Backend.Core.Entities.Branch", "Branch")
                        .WithMany()
                        .HasForeignKey("fk_Branch")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Core.Entities.Presentation", "Presentation")
                        .WithMany("Branches")
                        .HasForeignKey("fk_Presentation")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Core.Entities.Representative", b =>
                {
                    b.HasOne("Backend.Core.Entities.Booking")
                        .WithMany("Representatives")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Core.Entities.DataFile", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Core.Entities.ResourceBooking", b =>
                {
                    b.HasOne("Backend.Core.Entities.Booking", "Booking")
                        .WithMany("Resources")
                        .HasForeignKey("fk_Booking")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Core.Entities.Resource", "Resource")
                        .WithMany()
                        .HasForeignKey("fk_Resource")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Core.Entities.Tag", b =>
                {
                    b.HasOne("Backend.Core.Entities.Company")
                        .WithMany("Tags")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
