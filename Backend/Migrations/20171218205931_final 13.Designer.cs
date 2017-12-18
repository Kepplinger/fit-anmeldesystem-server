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
    [Migration("20171218205931_final 13")]
    partial class final13
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
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

                    b.Property<int?>("FK_Areas");

                    b.Property<string>("GraphicURL")
                        .IsRequired();

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("FK_Areas");

                    b.ToTable("Areas");
                });

            modelBuilder.Entity("Backend.Core.Entities.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AdditionalInfo");

                    b.Property<DateTime>("CreationDate");

                    b.Property<int>("FK_Branches");

                    b.Property<int>("FK_Company");

                    b.Property<int>("FK_Event");

                    b.Property<int>("FK_Location");

                    b.Property<int>("FK_Package");

                    b.Property<int>("FK_Presentation");

                    b.Property<int>("FK_Representatives");

                    b.Property<int>("FK_Resources");

                    b.Property<bool>("ProvidesSummerJob");

                    b.Property<bool>("ProvidesThesis");

                    b.Property<string>("Remarks");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("isAccepted");

                    b.HasKey("Id");

                    b.HasIndex("FK_Company");

                    b.HasIndex("FK_Event");

                    b.HasIndex("FK_Location");

                    b.HasIndex("FK_Package");

                    b.HasIndex("FK_Presentation");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("Backend.Core.Entities.Branch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("FK_Branch");

                    b.Property<int?>("FK_Branches");

                    b.Property<string>("Name");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("FK_Branch");

                    b.HasIndex("FK_Branches");

                    b.ToTable("Branches");
                });

            modelBuilder.Entity("Backend.Core.Entities.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Branch")
                        .IsRequired();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("EstablishmentsAut")
                        .IsRequired();

                    b.Property<int>("EstablishmentsCountAut");

                    b.Property<int>("EstablishmentsCountInt");

                    b.Property<string>("EstablishmentsInt");

                    b.Property<int>("FK_Address");

                    b.Property<int>("FK_Contact");

                    b.Property<string>("Homepage")
                        .IsRequired();

                    b.Property<string>("LogoUrl")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("PhoneNumber")
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

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("PhoneNumber");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("Backend.Core.Entities.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EventDate");

                    b.Property<int>("FK_Areas");

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

            modelBuilder.Entity("Backend.Core.Entities.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Category")
                        .IsRequired();

                    b.Property<int>("FK_Area");

                    b.Property<int>("Number");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<double>("XCoordinate");

                    b.Property<double>("YCoordinate");

                    b.HasKey("Id");

                    b.HasIndex("FK_Area");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Backend.Core.Entities.Presentation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(400);

                    b.Property<int>("FK_Branch");

                    b.Property<string>("FileURL");

                    b.Property<bool>("IsAccepted");

                    b.Property<string>("RoomNumber")
                        .IsRequired();

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

                    b.Property<string>("Email");

                    b.Property<int?>("FK_Representatives");

                    b.Property<string>("ImageUrl");

                    b.Property<string>("Name");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("FK_Representatives");

                    b.ToTable("Rerpresentatives");
                });

            modelBuilder.Entity("Backend.Core.Entities.Resource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<int?>("FK_Resources");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("FK_Resources");

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("Backend.Core.Entities.ResourceBooking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Amount");

                    b.Property<int>("FK_Booking");

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
                    b.HasOne("Backend.Core.Entities.Event")
                        .WithMany("Areas")
                        .HasForeignKey("FK_Areas")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Core.Entities.Booking", b =>
                {
                    b.HasOne("Backend.Core.Entities.Company", "Company")
                        .WithMany()
                        .HasForeignKey("FK_Company")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Core.Entities.Event", "Event")
                        .WithMany()
                        .HasForeignKey("FK_Event")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Core.Entities.Location", "Location")
                        .WithMany()
                        .HasForeignKey("FK_Location")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Core.Entities.FitPackage", "Package")
                        .WithMany()
                        .HasForeignKey("FK_Package")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Core.Entities.Presentation", "Presentation")
                        .WithMany()
                        .HasForeignKey("FK_Presentation")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Core.Entities.Branch", b =>
                {
                    b.HasOne("Backend.Core.Entities.Presentation")
                        .WithMany("Branches")
                        .HasForeignKey("FK_Branch")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Core.Entities.Booking")
                        .WithMany("Branches")
                        .HasForeignKey("FK_Branches")
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

            modelBuilder.Entity("Backend.Core.Entities.Location", b =>
                {
                    b.HasOne("Backend.Core.Entities.Area", "Area")
                        .WithMany()
                        .HasForeignKey("FK_Area")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Core.Entities.Representative", b =>
                {
                    b.HasOne("Backend.Core.Entities.Booking")
                        .WithMany("Representatives")
                        .HasForeignKey("FK_Representatives")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Core.Entities.Resource", b =>
                {
                    b.HasOne("Backend.Core.Entities.Booking")
                        .WithMany("Resources")
                        .HasForeignKey("FK_Resources")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Core.Entities.ResourceBooking", b =>
                {
                    b.HasOne("Backend.Core.Entities.Booking", "Booking")
                        .WithMany()
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
