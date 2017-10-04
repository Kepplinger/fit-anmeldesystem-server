using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;

namespace GR.Data
{
    public interface IRepository<T> where T : IEntityObject
    {
        IEnumerable<T> GetAll();
        T Get(long id);
        void Insert(T entity);        
        void Update(T entity);
        void Delete(T entity);
    }
}
