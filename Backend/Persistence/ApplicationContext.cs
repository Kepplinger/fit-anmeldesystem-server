using System.Linq;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Backend.Persistence
{
    public class ApplicationContext : DbContext
    {

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Detail> Details { get; set; }
        public DbSet<DetailAllocation> DetailAllocations { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Presentation> Presentations { get; set; }
        public DbSet<Rerpresentative> Rerpresentatives { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<ResourceBooking> ResourceBookings { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options){
        }

        public ApplicationContext() : base(){
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(modelBuilder);
        }

    }
}