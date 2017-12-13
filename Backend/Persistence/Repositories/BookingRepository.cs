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

        public Booking[] Get(Expression<Func<Booking, bool>> filter = null, Func<IQueryable<Booking>, IOrderedQueryable<Booking>> orderBy = null, string includeProperties = "", string includeLevelTwoProps = "")
        {
            IQueryable<Booking> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            // alle gewünschten abhängigen Entitäten mitladen
            foreach (string lvlTwoIncl in includeLevelTwoProps.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] inclKeyValue = lvlTwoIncl.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < inclKeyValue.Length; i++)
                {
                    query = query.Include(p => p.Company).Include(x => x.Company.Address);

                }
            }

            if (orderBy != null)
            {
                return orderBy(query).ToArray();
            }
            return query.ToArray();
        }
    }
}
