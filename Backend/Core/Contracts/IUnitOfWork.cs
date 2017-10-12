using Backend.Core.Contracts.Repositories;
using Backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Contracts
{
    public interface IUnitOfWork: IDisposable
    {
        
        /// <summary>
        /// Standard Repositories 
        /// </summary>
        IGenericRepository<Area> AreaRepository { get; }
        
        IGenericRepository<Booking> BookingRepository { get; }
        IGenericRepository<Category> CategoryRepository { get; }
        IGenericRepository<Company> CompanyRepository { get; }
        IGenericRepository<Contact> ContactRepository { get; }
        IGenericRepository<Detail> DetailRepository { get; }
        IGenericRepository<DetailAllocation> DetailAllocationRepository { get; }
        IGenericRepository<Event> EventRepository { get; }
        IGenericRepository<Lecturer> LecturerRepository { get; }
        IGenericRepository<Location> LocationRepository { get; }
        IGenericRepository<Person> PersonRepository { get; }
        IGenericRepository<Presentation> PresentationRepository { get; }
        IGenericRepository<Representative> RepresentativeRepository { get; }
        IGenericRepository<Resource> ResourceRepository { get; }
        IGenericRepository<ResourceBooking> ResourceBookingRepository { get; }
        /// <summary>
        /// Erweiterte Repositories
        /// </summary>
        IAddressRepository AddressRepository { get; }

        void Save();

        void DeleteDatabase();

        void FillDb();

    }
}
