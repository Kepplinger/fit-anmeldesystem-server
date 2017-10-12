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

        /// <summary>
        /// Erweiterte Repositories
        /// </summary>
        IAddressRepository AddressRepository { get; }

        void Save();

        void DeleteDatabase();

        void FillDb();

    }
}
