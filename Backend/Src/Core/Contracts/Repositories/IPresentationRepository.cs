using Backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Backend.Core.Contracts.Repositories {
    public interface IPresentationRepository : IGenericRepository<Presentation> {
        Presentation[] Get(Expression<Func<Presentation, bool>> filter = null, Func<IQueryable<Presentation>, IOrderedQueryable<Presentation>> orderBy = null, string includeProperties = "");
    }
}
