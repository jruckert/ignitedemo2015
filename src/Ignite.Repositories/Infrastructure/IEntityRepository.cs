namespace Ignite.Repositories.Infrastructure
{
    using Entities;
    using Microsoft.Data.Entity.ChangeTracking;
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IEntityRepository<T> where T : BaseEntity
    {
        IDbSession DbSession { get; }

        void Commit();

        T Create(T entity);

        void Update(T entity);

        void Delete(T entity);

        IQueryable<T> Find(Expression<Func<T, bool>> predicate);

        IQueryable<T> FindAll();

        T FindById(int id);

        bool Exists(int id);

        void ReloadEntity(T entity);

        ChangeTracker TrackChanges();
    }
}