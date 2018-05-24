using System;
using System.Linq;
using System.Linq.Expressions;
using Backend.Core.Entities;
namespace Backend.Core.Contracts.Repositories
{
    public interface IEventRepository : IGenericRepository<Event>
    {
        Event[] Get(Expression<Func<Event, bool>> filter = null, Func<IQueryable<Event>, IOrderedQueryable<Event>> orderBy = null, string includeProperties = "");

        Event GetCurrentEvent();
    }
}
