using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Backend.Core.Contracts
{
    public interface IGenericRepository<TEntity> where TEntity : class, IEntityObject, new()
    {
        /// <summary>
        ///     Liefert eine Menge von Entities zurück. Diese kann optional
        ///     gefiltert und/oder sortiert sein.
        ///     Weiters werden bei Bedarf abhängige Entitäten mitgeladen (eager loading).
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        TEntity[] Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        /// <summary>
        ///     Eindeutige Entität oder null zurückliefern
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetById(object id);

        /// <summary>
        /// Fügt neue Identität zum Datenbestand hinzu
        /// </summary>
        /// <param name="entity"></param>
        void Insert(TEntity entity);

        /// <summary>
        ///     Entität per primary key löschen
        /// </summary>
        /// <param name="id"></param>
        bool Delete(object id);

        /// <summary>
        ///     Übergebene Entität löschen.
        /// </summary>
        /// <param name="entityToDelete"></param>
        void Delete(TEntity entityToDelete);

        /// <summary>
        ///     Entität aktualisieren
        /// </summary>
        /// <param name="entityToUpdate"></param>
        void Update(TEntity entityToUpdate);

        /// <summary>
        ///     Generisches Count mit Filtermöglichkeit. Sind vom Filter
        ///     Navigationproperties betroffen, können diese per eager-loading
        ///     geladen werden.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        int Count(Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = "");

        /// <summary>
        ///     Liste von Entities einfügen
        /// </summary>
        /// <param name="entities"></param>
        void InsertMany(IEnumerable<TEntity> entities);
    }
}