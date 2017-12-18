using Backend.Core.Entities;
using FITBackend.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        //public List<Booking> getAllBookings()
        //{
        //    this.Get(includeProperties: "Company");
        //    return boookings;
        //}

        public Booking[] Get(Expression<Func<Booking, bool>> filter = null, Func<IQueryable<Booking>, IOrderedQueryable<Booking>> orderBy = null, string includeProperties = "")
        {
            IQueryable<Booking> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            query = query.Include(p => p.Company).Include(p => p.Company.Address).Include(p => p.Company.Contact).Include(p => p.Presentation.Branches).Include(p => p.Location.Area).Include(p => p.FitPackage).Include(p => p.Branches);

            if (orderBy != null)
            {
                return orderBy(query).ToArray();
            }
            return query.ToArray();
        }
    }
}
