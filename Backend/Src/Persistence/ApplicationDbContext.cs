using System.Linq;
using Backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Backend.Src.Core.Entities;
using Backend.Core.Entities.UserManagement;

namespace Backend.Persistence
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<LockPage> LockPages { get; set; }
        public DbSet<MemberStatus> MemberStati { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<ChangeProtocol> ChangeProtocols { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<FitPackage> Packages { get; set; }
        public DbSet<Presentation> Presentations { get; set; }
        public DbSet<Representative> Representatives { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<ResourceBooking> ResourceBookings { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Graduate> Graduates { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BookingBranch> BookingBranches { get; set; }
        public DbSet<PresentationBranch> PresentationBranches { get; set; }
        public DbSet<SmtpConfig> SmtpConfigs { get; set; }
        public DbSet<FitUser> FitUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();
            string connectionString = configuration["ConnectionStrings:ServerConnection"];
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(modelBuilder);
        }

    }
}