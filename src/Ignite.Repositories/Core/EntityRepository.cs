namespace Ignite.Repositories.Core
{
    using Entities;
    using Infrastructure;
    using Microsoft.Data.Entity;
    using Microsoft.Data.Entity.ChangeTracking;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class EntityRepository<T> : IEntityRepository<T>
        where T : BaseEntity
    {
        private readonly ILogger localLogger;

        public IDbSession DbSession { get; private set; }

        private DbSet<T> DbSet { get; set; }

        public EntityRepository(IDbSession session, ILoggerFactory factory)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            DbSession = session;
            DbSet = session.Current.GetDbSet<T>();
            localLogger = factory.CreateLogger<EntityRepository<T>>();
        }

        public void Commit()
        {
            localLogger.LogVerbose("Commit to Database");
            DbSession.Current.Commit();
        }

        public virtual T Create(T entity)
        {
            localLogger.LogVerbose("Creating Entity");
            entity.Version = 1;
            entity.CreationDate = DateTime.Now;
            EntityEntry<T> entityEntry = DbSet.Add(entity);
            return entityEntry.Entity;
        }

        public virtual void Update(T entity)
        {
            localLogger.LogVerbose("Updating Entity");
            if (DbSession.Current.IsNewEntity(entity))
            {
                localLogger.LogVerbose("Is a new entity");
                return;
            }
            entity.Version++;
            entity.ModificationDate = DateTime.Now;            
            DbSet.Attach(entity);
            DbSession.Current.SetModifiedEntityState(entity);
        }        

        public virtual void Delete(T entity)
        {
            localLogger.LogVerbose("Deleting an entity");
            if (DbSession.Current.IsDetachedEntityState(entity))
            {
                DbSet.Attach(entity);
            }
            localLogger.LogVerbose("Removing the entity");
            DbSet.Remove(entity);
        }

        public virtual IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {            
            localLogger.LogVerbose("Find with raw predicate");
            return DbSet.Where(predicate);
        }

        public virtual IQueryable<T> FindAll()
        {
            localLogger.LogVerbose($"Find all");
            return DbSet;
        }

        public virtual T FindById(int id)
        {
            localLogger.LogVerbose($"Find first or default with id {id}");
            return DbSet.FirstOrDefault(o => o.Id == id);
        }

        public virtual bool Exists(int id)
        {
            localLogger.LogVerbose($"If exists with id {id}");
            return DbSet.AsNoTracking().Count(o => o.Id == id) > 0;
        }

        public virtual void ReloadEntity(T entity)
        {
            localLogger.LogVerbose($"Reloading entity");
            DbSession.Current.Reload(entity);
        }

        public ChangeTracker TrackChanges()
        {
            localLogger.LogVerbose($"Begin Track Changes");
            return ((DbContext)DbSession.Current).ChangeTracker;
        }

    }
}
