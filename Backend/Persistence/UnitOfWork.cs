﻿using System;
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
using Microsoft.EntityFrameworkCore.Storage;

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
        public IGenericRepository<Branch> BranchRepository { get; }
        public IGenericRepository<Company> CompanyRepository { get; }
        public IGenericRepository<Contact> ContactRepository { get; }
        public IGenericRepository<Event> EventRepository { get; }
        public IGenericRepository<Location> LocationRepository { get; }
        public IGenericRepository<Package> PackageRepository { get; }
        public IGenericRepository<Presentation> PresentationRepository { get; }
        public IGenericRepository<Representative> RepresentativeRepository { get; }
        public IGenericRepository<Resource> ResourceRepository { get; }
        public IGenericRepository<ResourceBooking> ResourceBookingRepository { get; }
        public IGenericRepository<Address> AddressRepository { get; }


        /// <summary>
        ///     Konkrete Repositories. Mit Ableitung nötig
        /// </summary>

        public IBookingRepository BookingRepository { get; }


        public UnitOfWork() 
        {
            _context = new ApplicationDbContext();

            AreaRepository = new GenericRepository<Area>(_context);

            AddressRepository = new GenericRepository<Address>(_context);

            BookingRepository = new BookingRepository(_context);

            CompanyRepository = new GenericRepository<Company>(_context);

            ContactRepository = new GenericRepository<Contact>(_context);


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

        public IDbContextTransaction BeginTransaction() {
            return _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _context.Database.CommitTransaction();
        }

        public void FillDb()
        {
            throw new NotImplementedException();
        }
    }
}
