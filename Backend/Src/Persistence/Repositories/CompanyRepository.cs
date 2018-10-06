using Backend.Core.Contracts.Repositories;
using Backend.Core.Entities;
using FITBackend.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Backend.Persistence.Repositories {
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository {

        public CompanyRepository(DbContext context) : base(context) {
        }

        public override Company[] Get(Expression<Func<Company, bool>> filter = null, Func<IQueryable<Company>, IOrderedQueryable<Company>> orderBy = null, string includeProperties = "") {
            IQueryable<Company> query = _dbSet;
            if (filter != null) {
                query = query.Where(filter);
            }
            query = query
                .Include(p => p.Address)
                .Include(p => p.Contact)
                .Include(p => p.MemberStatus)
                .Include(p => p.Branches).ThenInclude(p => p.Branch)
                .Include(p => p.Tags).ThenInclude(p => p.Tag);

            if (orderBy != null) {
                return orderBy(query).ToArray();
            }
            return query.ToArray();
        }
    }
}
