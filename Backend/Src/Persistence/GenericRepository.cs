using Backend.Core.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FITBackend.Persistence
{
    /// <summary>
    /// Generische Zugriffsmethoden für eine Entität
    /// Werden spezielle Zugriffsmethoden benötigt, wird eine spezielle
    /// abgeleitete Klasse erstellt.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntityObject, new()
    {
        protected readonly DbContext _context; // Aktueller DbContext wird vom UnitOfWork übergeben
        protected readonly DbSet<TEntity> _dbSet; // Set der entsprechenden Entität im Context

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        protected DbContext Context
        {
            get { return _context; }
        }

        /// <summary>
        ///     Liefert eine Menge von Entities zurück. Diese kann optional
        ///     gefiltert und/oder sortiert sein.
        ///     Weiters werden bei Bedarf abhängige Entitäten mitgeladen (eager loading).
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public virtual TEntity[] Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            // alle gewünschten abhängigen Entitäten mitladen
            foreach (string includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (orderBy != null)
            {
                return orderBy(query).ToArray();
            }
            return query.ToArray();
        }

        /// <summary>
        ///     Eindeutige Entität oder null zurückliefern
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        /// <summary>
        ///     Entität per primary key löschen
        /// </summary>
        /// <param name="id"></param>
        public virtual bool Delete(object id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            if (entityToDelete != null)
            {
                Delete(entityToDelete);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        ///     Übergebene Entität löschen. Ist sie nicht im Context verwaltet,
        ///     vorher dem Context zur Verwaltung übergeben.
        /// </summary>
        /// <param name="entityToDelete"></param>
        public virtual void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        /// <summary>
        ///     Entität aktualisieren
        /// </summary>
        /// <param name="entityToUpdate"></param>
        public virtual void Update(TEntity entityToUpdate)
        {
            //Prüfen ob Entität bereits im aktuellen Context vorhanden (falls ja, muss vorher Detach auf Entität durchgeführt werden,
            //um Attach ausführen zu können)
            TEntity existingEntity = _dbSet.Local.Where(x => x.Id == entityToUpdate.Id).FirstOrDefault();
            if (existingEntity != null)
            {
                if (Context.Entry(entityToUpdate).State == EntityState.Added)
                    throw new DbUpdateException("Update performed on inserted but not commited dataset", default(Exception));
                Context.Entry(existingEntity).State = EntityState.Added;
                _dbSet.Local.Remove(existingEntity);
                EntityState state = Context.Entry(existingEntity).State;
                //if ((state == EntityState.Unchanged) || (state == EntityState.Modified))
                //{
                //    _dbSet.Detach
                //    ObjectContext objectContext =
                //    ((IObjectContextAdapter)_context).ObjectContext;

                //    objectContext.Detach(existingEntity);
                //}
            }
            _dbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        /// <summary>
        ///     Generisches Count mit Filtermöglichkeit. Sind vom Filter
        ///     Navigationproperties betroffen, können diese per eager-loading
        ///     geladen werden.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public int Count(Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            // alle gewünschten abhängigen Entitäten mitladen
            foreach (string includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            return query.Count();
        }

        /// <summary>
        ///     Liste von Entities in den Kontext übernehmen.
        ///     Enormer Performancegewinn im Vergleich zum Einfügen einzelner Sätze
        /// </summary>
        /// <param name="entities"></param>
        public void InsertMany(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        /// <summary>
        /// Diese Methode ist eine allgemeine Methode zum Kopieren von 
        /// oeffentlichen Objekteigenschaften.
        /// </summary>
        /// <param name="target">Zielobjekt</param>
        /// <param name="source">Quelleobjekt</param>
        public static void CopyProperties(object target, object source)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (source == null) throw new ArgumentNullException("source");
            Type sourceType = source.GetType();
            Type targetType = target.GetType();
            foreach (var piSource in sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                            .Where(pi => pi.CanRead == true))
            {
                if (!piSource.PropertyType.FullName.StartsWith("System.Collections.Generic.ICollection"))  // kein Navigationproperty
                {
                    PropertyInfo piTarget = null;
                    piTarget = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                         .Where(pi => pi.Name.Equals(piSource.Name)
                                                      && pi.PropertyType.Equals(piSource.PropertyType)
                                                      && pi.CanWrite == true).SingleOrDefault();
                    if (piTarget != null)
                    {
                        object value = piSource.GetValue(source, null);
                        piTarget.SetValue(target, value, null);
                    }
                }
            }
        }
    }
}
