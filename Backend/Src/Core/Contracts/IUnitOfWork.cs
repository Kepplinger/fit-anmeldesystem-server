using Backend.Core.Entities;
using Backend.Persistence.Repositories;
using System;
using Microsoft.EntityFrameworkCore.Storage;
using Backend.Core.Contracts.Repositories;
using Backend.Src.Core.Entities;
using Backend.Core.Entities.UserManagement;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Backend.Core.Contracts {
    public interface IUnitOfWork : IDisposable {

        /// <summary>
        /// Standard Repositories 
        /// </summary>
        IGenericRepository<Area> AreaRepository { get; }
        IGenericRepository<MemberStatus> MemberStatusRepository { get; }
        IGenericRepository<RegistrationState> RegistrationStateRepository { get; }
        IGenericRepository<LockPage> LockPageRepository { get; }
        IGenericRepository<Branch> BranchRepository { get; }
        IGenericRepository<Contact> ContactRepository { get; }
        IGenericRepository<Location> LocationRepository { get; }
        IGenericRepository<FitPackage> PackageRepository { get; }
        IGenericRepository<Representative> RepresentativeRepository { get; }
        IGenericRepository<Resource> ResourceRepository { get; }
        IGenericRepository<ResourceBooking> ResourceBookingRepository { get; }
        IGenericRepository<Address> AddressRepository { get; }
        IGenericRepository<DataFile> DataFileRepository { get; }
        IGenericRepository<ChangeProtocol> ChangeRepository { get; }
        IGenericRepository<EmailVariable> EmailVariableRepository { get; }
        IGenericRepository<EmailVariableUsage> EmailVariableUsageRepository { get; }
        IGenericRepository<Graduate> GraduateRepository { get; }
        IGenericRepository<Tag> TagRepository { get; }
        IGenericRepository<CompanyTag> CompanyTagRepository { get; }
        IGenericRepository<CompanyBranch> CompanyBranchRepository { get; }
        IGenericRepository<BookingBranch> BookingBranchesRepository { get; }
        IGenericRepository<PresentationBranch> PresentationBranchesRepository { get; }
        IGenericRepository<SmtpConfig> SmtpConfigRepository { get; }
        /// <summary>
        /// Erweiterte Repositories
        /// </summary>
        IEmailRepository EmailRepository { get; }
        IBookingRepository BookingRepository { get; }
        IEventRepository EventRepository { get; }
        ICompanyRepository CompanyRepository { get; }
        IPresentationRepository PresentationRepository { get; }

        void Save();

        void DeleteDatabase();

        Task FillDb(IServiceProvider provider);

        IDbContextTransaction BeginTransaction();

        void Commit();
    }
}
