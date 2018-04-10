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
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.Property<int?>("FK_Event");

                    b.Property<string>("GraphicUrl");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("FK_Event");

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

                    b.Property<string>("CompanyDescription");

                    b.Property<int?>("ContactId");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("EstablishmentsAut")
                        .HasMaxLength(30);

                    b.Property<int>("EstablishmentsCountAut");

                    b.Property<int>("EstablishmentsCountInt");

                    b.Property<string>("EstablishmentsInt")
                        .HasMaxLength(30);

                    b.Property<int>("FK_Company");

                    b.Property<int>("FK_Event");

                    b.Property<int>("FK_FitPackage");

                    b.Property<int?>("FK_Presentation");

                    b.Property<string>("Homepage")
                        .IsRequired();

                    b.Property<int?>("LocationId");

                    b.Property<string>("Logo");

                    b.Property<string>("PhoneNumber")
                        .IsRequired();

                    b.Property<bool>("ProvidesSummerJob");

                    b.Property<bool>("ProvidesThesis");

                    b.Property<string>("Remarks")
                        .HasMaxLength(500);

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("isAccepted");

                    b.HasKey("Id");

                    b.HasIndex("ContactId");

                    b.HasIndex("FK_Company");

                    b.HasIndex("FK_Event");

                    b.HasIndex("FK_FitPackage");

                    b.HasIndex("FK_Presentation");

                    b.HasIndex("LocationId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("Backend.Core.Entities.Branch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("BookingId");

                    b.Property<int?>("FK_Presentation");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("BookingId");

                    b.HasIndex("FK_Presentation");

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

                    b.Property<int>("FK_Address");

                    b.Property<int>("FK_Contact");

                    b.Property<bool>("IsPending");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("RegistrationToken")
                        .IsRequired();

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("FK_Address");

                    b.HasIndex("FK_Contact");

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

                    b.Property<int>("FK_Address");

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

                    b.HasKey("Id");

                    b.HasIndex("FK_Address");

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

                    b.Property<string>("FileURL");

                    b.Property<bool>("IsAccepted");

                    b.Property<string>("RoomNumber");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.ToTable("Presentations");
                });

            modelBuilder.Entity("Backend.Core.Entities.Representative", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("BookingId");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("ImageUrl")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("BookingId");

                    b.ToTable("Rerpresentatives");
                });

            modelBuilder.Entity("Backend.Core.Entities.Resource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<int?>("FK_Booking");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("FK_Booking");

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("Backend.Core.Entities.ResourceBooking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("FK_Booking");

                    b.Property<int>("FK_Resource");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("FK_Booking");

                    b.HasIndex("FK_Resource");

                    b.ToTable("ResourceBookings");
                });

            modelBuilder.Entity("Backend.Core.Entities.Area", b =>
                {
                    b.HasOne("Backend.Core.Entities.Event", "Event")
                        .WithMany("Areas")
                        .HasForeignKey("FK_Event")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Core.Entities.Booking", b =>
                {
                    b.HasOne("Backend.Core.Entities.Contact", "Contact")
                        .WithMany()
                        .HasForeignKey("ContactId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Core.Entities.Company", "Company")
                        .WithMany()
                        .HasForeignKey("FK_Company")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Core.Entities.Event", "Event")
                        .WithMany()
                        .HasForeignKey("FK_Event")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Core.Entities.FitPackage", "FitPackage")
                        .WithMany()
                        .HasForeignKey("FK_FitPackage")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Core.Entities.Presentation", "Presentation")
                        .WithMany()
                        .HasForeignKey("FK_Presentation")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Core.Entities.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Core.Entities.Branch", b =>
                {
                    b.HasOne("Backend.Core.Entities.Booking")
                        .WithMany("Branches")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Core.Entities.Presentation", "Presentation")
                        .WithMany("Branches")
                        .HasForeignKey("FK_Presentation")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Core.Entities.Company", b =>
                {
                    b.HasOne("Backend.Core.Entities.Address", "Address")
                        .WithMany()
                        .HasForeignKey("FK_Address")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Core.Entities.Contact", "Contact")
                        .WithMany()
                        .HasForeignKey("FK_Contact")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Core.Entities.Graduate", b =>
                {
                    b.HasOne("Backend.Core.Entities.Address", "Address")
                        .WithMany()
                        .HasForeignKey("FK_Address")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Core.Entities.Location", b =>
                {
                    b.HasOne("Backend.Core.Entities.Area")
                        .WithMany("Locations")
                        .HasForeignKey("AreaId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Core.Entities.Representative", b =>
                {
                    b.HasOne("Backend.Core.Entities.Booking")
                        .WithMany("Representatives")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Core.Entities.Resource", b =>
                {
                    b.HasOne("Backend.Core.Entities.Booking", "Booking")
                        .WithMany()
                        .HasForeignKey("FK_Booking")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Core.Entities.ResourceBooking", b =>
                {
                    b.HasOne("Backend.Core.Entities.Booking", "Booking")
                        .WithMany("Resources")
                        .HasForeignKey("FK_Booking")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Core.Entities.Resource", "Resource")
                        .WithMany()
                        .HasForeignKey("FK_Resource")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
