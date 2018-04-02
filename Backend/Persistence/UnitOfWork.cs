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
using SQLitePCL;
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
        public IGenericRepository<Branch> BranchRepository { get; }
        public IGenericRepository<Company> CompanyRepository { get; }
        public IGenericRepository<ChangeProtocol> ChangeRepository { get; }
        public IGenericRepository<Contact> ContactRepository { get; }
        public IGenericRepository<Location> LocationRepository { get; }
        public IGenericRepository<FitPackage> PackageRepository { get; }
        public IGenericRepository<Presentation> PresentationRepository { get; }
        public IGenericRepository<Representative> RepresentativeRepository { get; }
        public IGenericRepository<Resource> ResourceRepository { get; }
        public IGenericRepository<ResourceBooking> ResourceBookingRepository { get; }
        public IGenericRepository<Address> AddressRepository { get; }

        /// <summary>
        ///     Konkrete Repositories. Mit Ableitung nötig
        /// </summary>

        public IBookingRepository BookingRepository { get; }

        public IEventRepository EventRepository { get; }


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

            ResourceBookingRepository = new GenericRepository<ResourceBooking>(_context);

            ChangeRepository = new GenericRepository<ChangeProtocol>(_context);

            EventRepository = new EventRepository(_context);

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


            // Set up Company
            Company company = new Company();
            company.Name = "Kepplinger IT";
            company.IsPending = false;
            company.RegistrationToken = "FirmenToken1";

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

            // Set up Company
            company.Contact = contact;
            company.Address = address;

            _context.Companies.Add(company);

            _context.SaveChanges();

            //Set up Ressources
            Resource resource = new Resource();
            resource.Name = "Stuhl";
            resource.Description = "Braucht die Firma einen Stuhl";
            _context.Resources.Add(resource);
            Resource resource2 = new Resource();
            resource2.Name = "Fernseher";
            resource2.Description = "Die Firma braucht einen Fernseher";
            _context.Resources.Add(resource2);
            resource2 = new Resource();
            resource2.Name = "Stehtisch";
            resource2.Description = "Die Firma braucht einen Stehtisch";
            _context.Resources.Add(resource2);
            resource2 = new Resource();
            resource2.Name = "WLAN";
            resource2.Description = "Die Firma braucht WLAN";
            _context.Resources.Add(resource2);
            resource2 = new Resource();
            resource2.Name = "Strom";
            resource2.Description = "Die Firma braucht Strom";
            _context.Resources.Add(resource2);


            //Representatives
            List<Representative> repre = new List<Representative>();
            Representative repr = new Representative();
            repr.Email = "andi.sakal15@gmail.com";
            repr.ImageUrl = "iagendans";
            repr.Name = "Andrej Sakal";

            _context.Rerpresentatives.Add(repr);
            _context.SaveChanges();
            repre.Add(repr);

            FitPackage package = new FitPackage();
            package.Name = "Basispaket";
            package.Discriminator = 1;
            package.Description = "Das Grundpaket bietet Ihnen einen Standplatz am FIT";
            package.Price = 200;

            _context.Packages.Add(package);
            _context.SaveChanges();

            FitPackage package2 = new FitPackage();
            package2.Name = "Sponsorpaket";
            package2.Discriminator = 2;
            package2.Description = "Beim Sponsorpaket zusätzlich enthalten ist noch anbringung Ihres Firmenlogos auf Werbematerialien des FITs";
            package2.Price = 400;

            _context.Packages.Add(package2);
            _context.SaveChanges();

            FitPackage package3 = new FitPackage();
            package3.Name = "Vortragspaket";
            package3.Discriminator = 3;
            package3.Description = "Beim Vortragspaket zuästzlich zu den restlichen Paketen darf man einen Vortrag halten";
            package3.Price = 600;

            _context.Packages.Add(package3);
            _context.SaveChanges();

            Branch it = new Branch();
            it.Name = "Informatik/Medientechnik";

            _context.Branches.Add(it);
            _context.SaveChanges();

            Branch elektr = new Branch();
            elektr.Name = "Elektronik/techn. Informatik";

            _context.Branches.Add(elektr);
            _context.SaveChanges();

            Branch bio = new Branch();
            bio.Name = "Biomedizin & Gesundheitstechnik";

            _context.Branches.Add(bio);
            _context.SaveChanges();

            Event e = new Event();
            e.EventDate = DateTime.Now;
            e.RegistrationEnd = DateTime.Now.AddMonths(2);
            e.RegistrationStart = DateTime.Now.AddMonths(-2);
            e.IsLocked = false;
            e.IsCurrent = true;

            _context.Events.Add(e);
            _context.SaveChanges();


            for (int i = 0; i < 11; i++)
            {
                // Set up Booking
                Booking booking = new Booking();
                booking.AdditionalInfo = "Here is some Additional Info";
                booking.CompanyDescription = "This is the company description";
                booking.isAccepted = true;
                booking.ProvidesSummerJob = true;
                booking.ProvidesThesis = false;
                booking.Remarks = "Remark";
                booking.CreationDate = DateTime.Now;
                booking.FK_FitPackage = package.Id;
                booking.Event = e;
                booking.Representatives = repre;
                booking.FK_Company = company.Id;
                booking.Email = "officemail@gmail.com";
                booking.Branch = "Firmen Branche";
                booking.EstablishmentsAut = "Linz";
                booking.EstablishmentsCountAut = 1;
                booking.EstablishmentsCountInt = 0;
                booking.EstablishmentsInt = "";
                booking.Homepage = "www.fit.com";
                booking.Logo = "logo";
                booking.PhoneNumber = "firmenphonenr";

                booking.Resources = new List<Resource>();
                Resource rsb = new Resource();
                rsb.Name = "eine ressource";
                rsb.Description = "description einer ressource";
                _context.Resources.Add(rsb);
                booking.Resources.Add(rsb);

                ResourceBooking rb = new ResourceBooking();
                rb.Booking = booking;
                rb.Amount = 2;
                rb.Resource = rsb;

                _context.ResourceBookings.Add(rb);


                _context.Bookings.Add(booking);
                _context.SaveChanges();
            }


            var res = _context.Bookings
                              .Include(p => p.Event)
                              .Include(p => p.Branches)
                              .Include(p => p.FitPackage)
                              .Include(p => p.Company)
                              .Include(p => p.Location)
                              .Include(p => p.Representatives)
                              .Include(p => p.Resources);

            foreach (var emp in res)
            {
                Console.WriteLine("Testdaten eingefügt! \n" +
                                  "Firma: " + emp.Company.Name +
                                  "\nPackage: " + emp.FitPackage.Name +
                                  "\nAdditionalInfo: " + emp.AdditionalInfo);
            }
        }
    }
}
