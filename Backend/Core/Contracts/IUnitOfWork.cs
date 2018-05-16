using Backend.Core.Entities;
using Backend.Persistence.Repositories;
using System;
using Microsoft.EntityFrameworkCore.Storage;
using Backend.Core.Contracts.Repositories;

namespace Backend.Core.Contracts
{
    public interface IUnitOfWork : IDisposable
    {

        /// <summary>
        /// Standard Repositories 
        /// </summary>
        IGenericRepository<Area> AreaRepository { get; }
        IGenericRepository<Branch> BranchRepository { get; }
        IGenericRepository<Company> CompanyRepository { get; }
        IGenericRepository<Contact> ContactRepository { get; }
        IGenericRepository<Location> LocationRepository { get; }
        IGenericRepository<FitPackage> PackageRepository { get; }
        IGenericRepository<Presentation> PresentationRepository { get; }
        IGenericRepository<Representative> RepresentativeRepository { get; }
        IGenericRepository<Resource> ResourceRepository { get; }
        IGenericRepository<ResourceBooking> ResourceBookingRepository { get; }
        IGenericRepository<Address> AddressRepository { get; }
        IGenericRepository<ChangeProtocol> ChangeRepository { get; }
        IGenericRepository<Email> EmailRepository { get; }
        IGenericRepository<Graduate> GraduateRepository { get; }
        IGenericRepository<Tag> TagRepository { get; }
        IGenericRepository<BookingBranches> BookingBranchesRepository { get; }
        /// <summary>
        /// Erweiterte Repositories
        /// </summary>
        IBookingRepository BookingRepository { get; }
        IEventRepository EventRepository { get; }


        void Save();

        void DeleteDatabase();

        void FillDb();

        IDbContextTransaction BeginTransaction();

        void Commit();

    }
}
