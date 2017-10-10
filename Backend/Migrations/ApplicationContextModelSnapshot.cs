﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using vega.Persistence;

namespace Backend.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Backend.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(7);

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Backend.Models.Area", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Designation")
                        .IsRequired();

                    b.Property<int>("FK_Event");

                    b.Property<byte[]>("Graphic")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("FK_Event");

                    b.ToTable("Areas");
                });

            modelBuilder.Entity("Backend.Models.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FK_Category");

                    b.Property<int>("FK_Company");

                    b.Property<int>("FK_Event");

                    b.Property<int>("FK_Location");

                    b.Property<int>("FK_Presentation");

                    b.Property<bool>("isAccepted");

                    b.HasKey("Id");

                    b.HasIndex("FK_Category");

                    b.HasIndex("FK_Company");

                    b.HasIndex("FK_Event");

                    b.HasIndex("FK_Location");

                    b.HasIndex("FK_Presentation");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("Backend.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<int>("Fk_Location");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<double>("Price");

                    b.HasKey("Id");

                    b.HasIndex("Fk_Location");

                    b.ToTable("Categorys");
                });

            modelBuilder.Entity("Backend.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("CompanySign")
                        .IsRequired();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<int>("FK_Address");

                    b.Property<int>("FK_Contact");

                    b.Property<string>("Homepage")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("PhoneNumber")
                        .IsRequired();

                    b.Property<string>("ShortDescription")
                        .IsRequired()
                        .HasMaxLength(350);

                    b.Property<string>("SubjectAreas")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("FK_Address");

                    b.HasIndex("FK_Contact");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Backend.Models.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FK_Person");

                    b.HasKey("Id");

                    b.HasIndex("FK_Person");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("Backend.Models.Detail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.ToTable("Details");
                });

            modelBuilder.Entity("Backend.Models.DetailAllocation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FK_Booking");

                    b.Property<int>("FK_Detail");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.HasIndex("FK_Booking");

                    b.HasIndex("FK_Detail");

                    b.ToTable("DetailAllocations");
                });

            modelBuilder.Entity("Backend.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<bool>("IsLocked");

                    b.Property<DateTime>("RegistrationEnd");

                    b.Property<DateTime>("RegistrationStart");

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Backend.Models.Lecturer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FK_Person");

                    b.Property<int>("FK_Presentation");

                    b.HasKey("Id");

                    b.HasIndex("FK_Person");

                    b.HasIndex("FK_Presentation");

                    b.ToTable("Lecturers");
                });

            modelBuilder.Entity("Backend.Models.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FK_Area");

                    b.Property<int>("Number");

                    b.Property<double>("XCoordinate");

                    b.Property<double>("YCoordinate");

                    b.HasKey("Id");

                    b.HasIndex("FK_Area");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Backend.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.Property<string>("PhoneNumber")
                        .IsRequired();

                    b.Property<byte[]>("Picture")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("Backend.Models.Presentation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(400);

                    b.Property<bool>("IsAccepted");

                    b.Property<string>("RoomNumber")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.ToTable("Presentations");
                });

            modelBuilder.Entity("Backend.Models.Rerpresentative", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FK_Booking");

                    b.Property<int>("FK_Person");

                    b.HasKey("Id");

                    b.HasIndex("FK_Booking");

                    b.HasIndex("FK_Person");

                    b.ToTable("Rerpresentatives");
                });

            modelBuilder.Entity("Backend.Models.Resource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.HasKey("Id");

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("Backend.Models.ResourceBooking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Amount");

                    b.Property<int>("FK_Booking");

                    b.Property<int>("FK_Resource");

                    b.HasKey("Id");

                    b.HasIndex("FK_Booking");

                    b.HasIndex("FK_Resource");

                    b.ToTable("ResourceBookings");
                });

            modelBuilder.Entity("Backend.Models.Area", b =>
                {
                    b.HasOne("Backend.Models.Event", "Event")
                        .WithMany()
                        .HasForeignKey("FK_Event")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Models.Booking", b =>
                {
                    b.HasOne("Backend.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("FK_Category")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("FK_Company")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Models.Event", "Event")
                        .WithMany()
                        .HasForeignKey("FK_Event")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Models.Location", "Location")
                        .WithMany()
                        .HasForeignKey("FK_Location")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Models.Presentation", "Presentation")
                        .WithMany()
                        .HasForeignKey("FK_Presentation")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Models.Category", b =>
                {
                    b.HasOne("Backend.Models.Location", "Location")
                        .WithMany()
                        .HasForeignKey("Fk_Location")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Models.Company", b =>
                {
                    b.HasOne("Backend.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("FK_Address")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Models.Contact", "Contact")
                        .WithMany()
                        .HasForeignKey("FK_Contact")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Models.Contact", b =>
                {
                    b.HasOne("Backend.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("FK_Person")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Models.DetailAllocation", b =>
                {
                    b.HasOne("Backend.Models.Booking", "Booking")
                        .WithMany()
                        .HasForeignKey("FK_Booking")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Models.Detail", "Detail")
                        .WithMany()
                        .HasForeignKey("FK_Detail")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Models.Lecturer", b =>
                {
                    b.HasOne("Backend.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("FK_Person")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Models.Presentation", "Presentation")
                        .WithMany()
                        .HasForeignKey("FK_Presentation")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Models.Location", b =>
                {
                    b.HasOne("Backend.Models.Area", "Area")
                        .WithMany()
                        .HasForeignKey("FK_Area")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Models.Rerpresentative", b =>
                {
                    b.HasOne("Backend.Models.Booking", "Booking")
                        .WithMany()
                        .HasForeignKey("FK_Booking")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("FK_Person")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Models.ResourceBooking", b =>
                {
                    b.HasOne("Backend.Models.Booking", "Booking")
                        .WithMany()
                        .HasForeignKey("FK_Booking")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Models.Resource", "Resource")
                        .WithMany()
                        .HasForeignKey("FK_Resource")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
