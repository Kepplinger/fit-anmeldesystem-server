using Backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Backend.Core.Contracts.Repositories
{
    public interface IEmailRepository : IGenericRepository<Email>
    {
       Email[] Get(Expression<Func<Email, bool>> filter = null, Func<IQueryable<Email>, IOrderedQueryable<Email>> orderBy = null, string includeProperties = "");
    }
}
