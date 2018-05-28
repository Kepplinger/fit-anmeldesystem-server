using Backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Backend.Core.Contracts.Repositories
{
    public interface ICompanyRepository : IGenericRepository<Company> {
        Company[] Get(Expression<Func<Company, bool>> filter = null, Func<IQueryable<Company>, IOrderedQueryable<Company>> orderBy = null, string includeProperties = "");
    }
}
