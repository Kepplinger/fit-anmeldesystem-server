using Backend.Core.Entities;
using Backend.Persistence.Repositories;
using System;
using Microsoft.EntityFrameworkCore.Storage;

namespace Backend.Core.Contracts
{
    public interface IUnitOfWork: IDisposable
    {
        
        /// <summary>
        /// Standard Repositories 
        /// </summary>
        IGenericRepository<Area> AreaRepository { get; }
        
        IGenericRepository<Branch> BranchRepository { get; }
        IGenericRepository<Company> CompanyRepository { get; }
        IGenericRepository<Contact> ContactRepository { get; }
        IGenericRepository<Event> EventRepository { get; }
        IGenericRepository<Location> LocationRepository { get; }
        IGenericRepository<FitPackage> PackageRepository { get; }
        IGenericRepository<Presentation> PresentationRepository { get; }
        IGenericRepository<Representative> RepresentativeRepository { get; }
        IGenericRepository<Resource> ResourceRepository { get; }
        IGenericRepository<ResourceBooking> ResourceBookingRepository { get; }
        IGenericRepository<Address> AddressRepository { get; }

        /// <summary>
        /// Erweiterte Repositories
        /// </summary>
        IBookingRepository BookingRepository { get; }


        void Save();

        void DeleteDatabase();

        void FillDb();

        IDbContextTransaction BeginTransaction();

        void Commit();

    }
}
