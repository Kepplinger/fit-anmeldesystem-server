using Backend.Core.Entities;
using FITBackend.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Backend.Persistence.Repositories
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        public List<Booking> getAllBookings()
        {
            return _context.Bookings.Get
        }
    }
}
