using System;
using System.Linq;
using System.Linq.Expressions;
using Backend.Core.Contracts.Repositories;
using Backend.Core.Entities;
using FITBackend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Backend.Persistence.Repositories
{
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        private ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        public Event[] Get(Expression<Func<Event, bool>> filter = null, Func<IQueryable<Event>, IOrderedQueryable<Event>> orderBy = null, string includeProperties = "")
        {
            IQueryable<Event> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            query = query.Include(p => p.Areas).ThenInclude(p => p.Locations);

            if (orderBy != null)
            {
                return orderBy(query).ToArray();
            }
            return query.ToArray();
        }
    }
}
