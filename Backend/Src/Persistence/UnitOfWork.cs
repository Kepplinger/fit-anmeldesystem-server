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
using System.Diagnostics;
using Backend.Core;
using Backend.Utils;
using Backend.Src.Core.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Backend.Core.Entities.UserManagement;
using System.Threading.Tasks;
using System.Security.Claims;

namespace StoreService.Persistence {

    public class UnitOfWork : IUnitOfWork {

        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        private bool _disposed;

        /// <summary>
        ///     Konkrete Standard-Repositories. Keine Ableitung nötig
        /// </summary>
        public IGenericRepository<Area> AreaRepository { get; }
        public IGenericRepository<RegistrationState> RegistrationStateRepository { get; }
        public IGenericRepository<LockPage> LockPageRepository { get;  }
        public IGenericRepository<Branch> BranchRepository { get; }
        public IGenericRepository<MemberStatus> MemberStatusRepository { get; }
        public IGenericRepository<ChangeProtocol> ChangeRepository { get; }
        public IGenericRepository<Contact> ContactRepository { get; }
        public IGenericRepository<Location> LocationRepository { get; }
        public IGenericRepository<FitPackage> PackageRepository { get; }
        public IGenericRepository<Representative> RepresentativeRepository { get; }
        public IGenericRepository<Resource> ResourceRepository { get; }
        public IGenericRepository<ResourceBooking> ResourceBookingRepository { get; }
        public IGenericRepository<Address> AddressRepository { get; }
        public IGenericRepository<DataFile> DataFileRepository { get; }
        public IGenericRepository<EmailVariable> EmailVariableRepository { get; }
        public IGenericRepository<EmailVariableUsage> EmailVariableUsageRepository { get; }
        public IGenericRepository<Tag> TagRepository { get; }
        public IGenericRepository<CompanyTag> CompanyTagRepository { get; }
        public IGenericRepository<CompanyBranch> CompanyBranchRepository { get; }
        public IGenericRepository<Graduate> GraduateRepository { get; }
        public IGenericRepository<BookingBranch> BookingBranchesRepository { get; }
        public IGenericRepository<PresentationBranch> PresentationBranchesRepository { get; }
        public IGenericRepository<SmtpConfig> SmtpConfigRepository { get; }

        /// <summary>
        ///     Konkrete Repositories. Mit Ableitung nötig
        /// </summary>
        public IEmailRepository EmailRepository { get; }

        public IBookingRepository BookingRepository { get; }

        public IEventRepository EventRepository { get; }

        public ICompanyRepository CompanyRepository { get; }

        public IPresentationRepository PresentationRepository { get; }


        public UnitOfWork() {

            _context = new ApplicationDbContext();

            AreaRepository = new GenericRepository<Area>(_context);
            RegistrationStateRepository = new GenericRepository<RegistrationState>(_context);
            AddressRepository = new GenericRepository<Address>(_context);
            LockPageRepository = new GenericRepository<LockPage>(_context);
            BookingRepository = new BookingRepository(_context);
            CompanyRepository = new CompanyRepository(_context);
            MemberStatusRepository = new GenericRepository<MemberStatus>(_context);
            ContactRepository = new GenericRepository<Contact>(_context);
            RepresentativeRepository = new GenericRepository<Representative>(_context);
            BranchRepository = new GenericRepository<Branch>(_context);
            LocationRepository = new GenericRepository<Location>(_context);
            PackageRepository = new GenericRepository<FitPackage>(_context);
            ResourceRepository = new GenericRepository<Resource>(_context);
            ResourceBookingRepository = new GenericRepository<ResourceBooking>(_context);
            ChangeRepository = new GenericRepository<ChangeProtocol>(_context);
            DataFileRepository = new GenericRepository<DataFile>(_context);
            EventRepository = new EventRepository(_context);
            EmailRepository = new EmailRepository(_context);
            PresentationRepository = new PresentationRepository(_context);
            EmailVariableRepository = new GenericRepository<EmailVariable>(_context);
            EmailVariableUsageRepository = new GenericRepository<EmailVariableUsage>(_context);
            GraduateRepository = new GenericRepository<Graduate>(_context);
            TagRepository = new GenericRepository<Tag>(_context);
            CompanyTagRepository = new GenericRepository<CompanyTag>(_context);
            CompanyBranchRepository = new GenericRepository<CompanyBranch>(_context);
            BookingBranchesRepository = new GenericRepository<BookingBranch>(_context);
            PresentationBranchesRepository = new GenericRepository<PresentationBranch>(_context);
            SmtpConfigRepository = new GenericRepository<SmtpConfig>(_context);
        }

        /// <summary>
        ///     Repository-übergreifendes Speichern der Änderungen
        /// </summary>
        public void Save() {
            _context.SaveChanges();
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (!_disposed) {
                if (disposing) {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void DeleteDatabase() {
            _context.Database.EnsureDeleted();
        }

        public void MigrateDatabase() {
            try {
                _context.Database.Migrate();
            } catch (Exception ex) {

                throw ex;
            }
        }

        public IDbContextTransaction BeginTransaction() {
            return _context.Database.BeginTransaction();
        }

        public void Commit() {
            _context.Database.CommitTransaction();
        }

        public async Task FillDb(IServiceProvider provider) {

            Console.WriteLine("\n\n=====================================================================================================================");
            Console.WriteLine("========================= DATABASE INITIALIZING =====================================================================");
            Console.WriteLine("=====================================================================================================================");

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Console.WriteLine("Delete Database ...");
            DeleteDatabase();
            Console.WriteLine("Make Migrations ...");
            MigrateDatabase();

            Save();

            using (var scope = provider.GetRequiredService<IServiceScopeFactory>().CreateScope()) {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<FitUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                string[] roles = new string[] { "FitAdmin", "FitReadOnly", "MemberAdmin", "MemberReadOnly", "Member" };

                foreach (var role in roles) {
                    if (!await roleManager.RoleExistsAsync(role)) {
                        IdentityRole newRole = new IdentityRole() { Name = role };
                        await roleManager.CreateAsync(newRole);
                        await roleManager.AddClaimAsync(newRole, new Claim("rol", role));
                    }
                }

                Save();

                // Creates some TestData
                await FillDbHelper.createTestData(_context, userManager);
                FillDbHelper.createEmails(this);
            }

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.WriteLine("=====================================================================================================================");
            Console.WriteLine("========================= DATABASE OPERATION SUCCESFULL TOOK " + elapsedTime + " ===============================================");
            Console.WriteLine("=====================================================================================================================");
        }
    }
}
