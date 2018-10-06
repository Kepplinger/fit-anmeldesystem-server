using Backend.Core.Entities;
using FITBackend.Persistence;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Backend.Persistence.Repositories
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        public Booking[] Get(Expression<Func<Booking, bool>> filter = null, Func<IQueryable<Booking>, IOrderedQueryable<Booking>> orderBy = null, string includeProperties = "")
        {
            IQueryable<Booking> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            query = query.Include(p => p.Company)
                         .Include(p => p.Company.Address)
                         .Include(p => p.Company.Contact)
                         .Include(p => p.Company.MemberStatus)
                         .Include(p => p.Contact)
                         .Include(p => p.Location)
                         .Include(p => p.Logo)
                         .Include(p => p.FitPackage)
                         .Include(p => p.Presentation)
                         .Include(P => P.Presentation.File)
                         .Include(p => p.Presentation.Branches).ThenInclude(p => p.Branch)
                         .Include(p => p.Branches).ThenInclude(p => p.Branch)
                         .Include(p => p.Representatives).ThenInclude(p => p.Image)
                         .Include(p => p.Event).ThenInclude(p => p.Areas)
                         .Include(p => p.Event).ThenInclude(p => p.Areas).ThenInclude(p => p.Locations)
                         .Include(p => p.Event).ThenInclude(p => p.RegistrationState)
                         .Include(p => p.Resources).ThenInclude(p => p.Resource);
            if (orderBy != null)
            {
                return orderBy(query).ToArray();
            }
            return query.ToArray();
        }

        public Representative[] GetRepresentativesOfBooking(Booking booking) {
            return _context.Bookings.Where(b => b.Id == booking.Id).Select(b => b.Representatives).FirstOrDefault().ToArray();
        }
    }
}
