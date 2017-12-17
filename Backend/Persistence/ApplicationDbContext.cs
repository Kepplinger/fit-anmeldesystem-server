using System.Linq;
using Backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;

namespace Backend.Persistence
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Presentation> Presentations { get; set; }
        public DbSet<Representative> Rerpresentatives { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<ResourceBooking> ResourceBookings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();
            Debug.Write(configuration.ToString());
            string connectionString = configuration["ConnectionStrings:Sakal"];
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