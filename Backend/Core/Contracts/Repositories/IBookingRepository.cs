using Backend.Core.Contracts;
using Backend.Core.Entities;
using System.Collections.Generic;

namespace Backend.Persistence.Repositories
{
    internal interface IBookingRepository: IGenericRepository<Booking>
    {
        List<Booking> getAllBookings();
    }
}