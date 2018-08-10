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

    public class PresentationRepository : GenericRepository<Presentation>, IPresentationRepository {
        private ApplicationDbContext _context;

        public PresentationRepository(ApplicationDbContext context) : base(context) {
            this._context = context;
        }

        public Presentation[] Get(Expression<Func<Presentation, bool>> filter = null, Func<IQueryable<Presentation>, IOrderedQueryable<Presentation>> orderBy = null, string includeProperties = "") {
            IQueryable<Presentation> query = _dbSet;
            if (filter != null) {
                query = query.Where(filter);
            }
            query = query.Include(p => p.File)
                         .Include(p => p.Branches).ThenInclude(p => p.Branch);

            if (orderBy != null) {
                return orderBy(query).ToArray();
            }
            return query.ToArray();
        }
    }
}
