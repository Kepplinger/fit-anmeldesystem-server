using Backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Core.Contracts.Repositories
{
    public interface IAddressRepository: IGenericRepository<Address>
    {
        List<Address> FindAddressByXYZ();
    }
}
