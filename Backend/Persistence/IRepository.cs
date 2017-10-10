using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Data
{
    public interface IRepository<T> where T : IEntityObject
    {
        IQueryable<T> GetAll(string[] includeProperties = null);
        T Get(long id);
        void Insert(T entity);        
        void Update(T entity);
        void Delete(T entity);

        void Save();
    }
}
