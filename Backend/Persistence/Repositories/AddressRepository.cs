using Backend.Core.Contracts.Repositories;
using Backend.Core.Entities;
using FITBackend.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Persistence.Repositories
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        ApplicationDbContext _context; 

        public List<Address> FindAddressByXYZ()
        {
            return null;
        }

        public AddressRepository(ApplicationDbContext context):base(context)
        {
            this._context = context;
        }
    }
}
