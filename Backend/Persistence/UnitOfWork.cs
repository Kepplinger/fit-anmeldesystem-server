using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using Backend.Core.Contracts;
using Backend.Persistence;
using Backend.Core.Entities;
using FITBackend.Persistence;
using Microsoft.EntityFrameworkCore;
using Backend.Persistence.Repositories;
using Backend.Core.Contracts.Repositories;

namespace StoreService.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        private bool _disposed;


        /// <summary>
        ///     Konkrete Standard-Repositories. Keine Ableitung nötig
        /// </summary>
        public IGenericRepository<Area> AreaRepository { get; }
        public IGenericRepository<Booking> BookingRepository { get; }
        public IGenericRepository<Category> CategoryRepository { get; }
        public IGenericRepository<Company> CompanyRepository { get; }
        public IGenericRepository<Contact> ContactRepository { get; }
        public IGenericRepository<Detail> DetailRepository { get; }
        public IGenericRepository<DetailAllocation> DetailAllocationRepository { get; }
        public IGenericRepository<Event> EventRepository { get; }
        public IGenericRepository<Lecturer> LecturerRepository { get; }
        public IGenericRepository<Location> LocationRepository { get; }
        public IGenericRepository<Person> PersonRepository { get; }
        public IGenericRepository<Presentation> PresentationRepository { get; }
        public IGenericRepository<Representative> RepresentativeRepository { get; }
        public IGenericRepository<Resource> ResourceRepository { get; }
        public IGenericRepository<ResourceBooking> ResourceBookingRepository { get; }

        /// <summary>
        ///     Konkrete Repositories. Mit Ableitung nötig
        /// </summary>
        public IAddressRepository AddressRepository { get; }

        public UnitOfWork() 
        {
            _context = new ApplicationDbContext();

            AreaRepository = new GenericRepository<Area>(_context);

            AddressRepository = new AddressRepository(_context);

            BookingRepository = new GenericRepository<Booking>(_context);
        }

        /// <summary>
        ///     Repository-übergreifendes Speichern der Änderungen
        /// </summary>
        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void DeleteDatabase()
        {
            _context.Database.EnsureDeleted();
        }

        public void FillDb()
        {

        }

    }
}
