using System.Linq;
using Backend.Data;
using Backend.Entities;
using Backend.Persistence;

namespace Backend.Repositories
{
    public class BookingRepository : IRepository<Booking>
    {
        private IRepository<Booking> _bookingRepo;

        public BookingRepository(ApplicationContext cb) {
            _bookingRepo = new Repository<Booking>(cb);
        }

        public void Delete(Booking entity)
        {
            _bookingRepo.Delete(entity);
        }

        public Booking Get(long id)
        {
            return _bookingRepo.Get(id);
        }

        public IQueryable<Booking> GetAll(string[] includeProperties = null)
        {
            return _bookingRepo.GetAll(includeProperties);
        }

        public void Insert(Booking entity)
        {
            _bookingRepo.Insert(entity);
        }

        public void Save()
        {
            _bookingRepo.Save();
        }

        public void Update(Booking entity)
        {
            _bookingRepo.Update(entity);
        }
    }
}