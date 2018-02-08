using System;
using Backend.Core.Contracts;
using Backend.Persistence;
using Backend.Core.Entities;
using FITBackend.Persistence;
using Backend.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
        public IGenericRepository<ChangeProtocol> ChangeRepository { get; }
        public IGenericRepository<Contact> ContactRepository { get; }
        public IGenericRepository<Event> EventRepository { get; }
        public IGenericRepository<Location> LocationRepository { get; }
        public IGenericRepository<FitPackage> PackageRepository { get; }
        public IGenericRepository<Presentation> PresentationRepository { get; }
        public IGenericRepository<Representative> RepresentativeRepository { get; }
        public IGenericRepository<Resource> ResourceRepository { get; }
        public IGenericRepository<ResourceBooking> ResourceBookingRepository { get; }
        public IGenericRepository<Address> AddressRepository { get; }
        public IGenericRepository<FolderInfo> FolderInfoRepository { get; set; }


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

            RepresentativeRepository = new GenericRepository<Representative>(_context);

            BranchRepository = new GenericRepository<Branch>(_context);

            LocationRepository = new GenericRepository<Location>(_context);

            PackageRepository = new GenericRepository<FitPackage>(_context);

            ResourceRepository = new GenericRepository<Resource>(_context);

            ChangeRepository = new GenericRepository<ChangeProtocol>(_context);

            EventRepository = new GenericRepository<Event>(_context);

            FolderInfoRepository = new GenericRepository<FolderInfo>(_context);

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

        public void MigrateDatabase()
        {
            try
            {
                _context.Database.Migrate();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _context.Database.CommitTransaction();
        }

        public void FillDb()
        {
            DeleteDatabase();
            MigrateDatabase();

            // Set up Booking
            Booking booking = new Booking();
            booking.AdditionalInfo = "Here is some Additional Info";
            booking.CompanyDescription = "This is the company description";
            booking.isAccepted = true;
            booking.ProvidesSummerJob = true;
            booking.ProvidesThesis = false;
            booking.Remarks = "Remark";

            // Set up Company
            Company company = new Company();
            company.Name = "Kepplinger IT";
            company.IsPending = false;
            company.RegistrationToken = "IWASDTOASD";

            // Set up Address
            Address address = new Address();
            address.Addition = "Additional Address Info";
            address.City = "Linz";
            address.StreetNumber = "14";
            address.Street = "some Street";
            address.ZipCode = "4020";

            _context.Addresses.Add(address);
            _context.SaveChanges();


            // Set Up Contact
            Contact contact = new Contact();
            contact.FirstName = "Andrej";
            contact.LastName = "Sakal";
            contact.Gender = "M";
            contact.PhoneNumber = "+4369917209297";
            contact.Email = "andi.sakal15@gmail.com";

            _context.Contacts.Add(contact);
            _context.SaveChanges();


            //Set up FitFolderInfo
            FolderInfo folderInfo = new FolderInfo();
            folderInfo.Email = "officemail@gmail.com";
            folderInfo.Branch = "Firmen Branche";
            folderInfo.EstablishmentsAut = "Linz";
            folderInfo.EstablishmentsCountAut = 1;
            folderInfo.EstablishmentsCountInt = 0;
            folderInfo.EstablishmentsInt = "";
            folderInfo.Homepage = "www.fit.com";
            folderInfo.Logo = "logo";
            folderInfo.PhoneNumber = "firmenphonenr";

            _context.FolderInfos.Add(folderInfo);
            _context.SaveChanges();


            // Set up Company
            company.Contact = contact;
            company.Address = address;
            company.FolderInfo = folderInfo;

            _context.Companies.Add(company);

            _context.SaveChanges();

            //Set up Ressources
            ResourceBooking resourceBooking = new ResourceBooking();
            resourceBooking.Amount = 1;

            List<Resource> resources = new List<Resource>();
            Resource resource = new Resource();
            resource.Name = "Stuhl";
            resource.Description = "Braucht die Firma einen Stuhl";
            resources.Add(resource);

            resourceBooking.Resource = resource;

            booking.Resources = resources;
            booking.Company = company;

            //Representatives
            Representative repr = new Representative();
            repr.Email = "andi.sakal15@gmail.com";
            repr.Image = "iagendans";
            repr.Name = "Andrej Sakal";





            _context.Bookings.Add(new Booking());


            var res = _context.Bookings.ToList();
            foreach (var emp in res)
            {
                Console.WriteLine(emp.Company.Name);
            }
        }
    }
}
