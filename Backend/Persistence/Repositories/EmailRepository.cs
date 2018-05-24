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
    public class EmailRepository : GenericRepository<Email>, IEmailRepository {

        public EmailRepository(DbContext context) : base(context) {
        }

        public Email[] Get(Expression<Func<Email, bool>> filter = null, Func<IQueryable<Email>, IOrderedQueryable<Email>> orderBy = null, string includeProperties = "") {
            IQueryable<Email> query = _dbSet;
            if (filter != null) {
                query = query.Where(filter);
            }
            query = query.Include(p => p.AvailableVariables).ThenInclude(p => p.EmailVariable);

            if (orderBy != null) {
                return orderBy(query).ToArray();
            }
            return query.ToArray();
        }
    }
}
