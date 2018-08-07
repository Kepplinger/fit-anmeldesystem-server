using System;
using System.Collections.Generic;
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
            query = query
                .Include(p => p.RegistrationState)
                .Include(p => p.Areas).ThenInclude(p => p.Locations)
                .Include(p => p.Areas).ThenInclude(p => p.Graphic);

            if (orderBy != null)
            {
                return orderBy(query).ToArray();
            }
            return query.ToArray();
        }

        /// <summary>
        /// Returns the current Event. 
        /// Current Event is either the nearest one in future, or the nearest one in past (past -> only if there is future event)
        /// </summary>
        /// <returns></returns>
        public Event GetCurrentEvent() {

            DateTime today = DateTime.Now;
            DateTime currentEventDate = DateTime.MinValue;

            // get future Event
            var query = _context.Events.Where(e => DateTime.Compare(e.EventDate.Date, today.Date) >= 0);

            if (query.Count() > 0) {
                currentEventDate = query.Min(e => e.EventDate);
            }

            // if no future Event exists
            if (currentEventDate == DateTime.MinValue) {
                currentEventDate = _context.Events.Max(e => e.EventDate);
            }

            return _context.Events.Where(e => e.EventDate == currentEventDate).Include("RegistrationState").FirstOrDefault();
        }
    }
}
