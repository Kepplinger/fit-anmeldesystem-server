using Backend.Core.Contracts;
using Backend.Core.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Backend.Persistence.Repositories
{
    public interface IBookingRepository: IGenericRepository<Booking>
    {
        Booking[] Get(Expression<Func<Booking, bool>> filter = null, Func<IQueryable<Booking>, IOrderedQueryable<Booking>> orderBy = null, string includeProperties = "");

        Representative[] GetRepresentativesOfBooking(Booking booking);
    }
}