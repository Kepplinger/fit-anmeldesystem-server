using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Backend.Persistence;
using Backend.Data;

namespace Backend.Data
{
    public class Repository<T> : IRepository<T> where T : IEntityObject
    {
        private readonly ApplicationContext context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;


        public DbSet<T> Entities() {
            if(entities!=null) {
                return entities;
            }
            return null;
        }

        public Repository(ApplicationContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }


        public T Get(long id)
        {
            return entities.SingleOrDefault(s => s.Id == id);
        }
        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.SaveChanges();
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAll(string[] includeProperties = null)
        {
            int i = 0;
            IQueryable<T> queryable = entities;
            while(includeProperties.Length > 0 && i < includeProperties.Length) {
                queryable = queryable.Include(includeProperties.ElementAt(i).ToString());
                i++;
            }
            
            return queryable;
        }

        /*private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }*/

    }
}
