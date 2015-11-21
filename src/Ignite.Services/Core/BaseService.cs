namespace Ignite.Services.Core
{
    using Entities;
    using Repositories.Infrastructure;
    using Infrastructure;
    using Microsoft.Extensions.Logging;
    using System.Linq;

    public class BaseService<T> : IBaseService<T>
        where T : BaseEntity
    {
        private readonly IEntityRepository<T> localRepository;
        private readonly ILogger localLogger;

        public BaseService(ILoggerFactory loggerFactory, IEntityRepository<T> repository)
        {
            localRepository = repository;
            localLogger = loggerFactory.CreateLogger<IEntityRepository<T>>();
        }

        public virtual T FindById(int id)
        {
            localLogger.LogVerbose($"Finding by id : {id}");
            return localRepository.FindById(id);
        }
        
        public virtual IQueryable<T> FindAll()
        {
            localLogger.LogVerbose("Finding all entities");
            return localRepository.FindAll();
        }
        
        public virtual bool Exists(int id)
        {
            localLogger.LogVerbose($"Checking if {id} exists.");
            return localRepository.Exists(id);
        }

        public virtual bool Update(T entity)
        {
            localLogger.LogVerbose("Checking if entity exists.");
            if (localRepository.Exists(entity.Id))
            {
                localLogger.LogVerbose("Updating entity.");
                localRepository.Update(entity);
                localRepository.Commit();
                return true;
            }
            localLogger.LogVerbose("Cannot find entity.");
            return false;
        }

        public virtual T Create(T entity)
        {
            localLogger.LogInformation("Creating new entity");
            var newEntity = localRepository.Create(entity);
            localRepository.Commit();
            return localRepository.FindById(entity.Id);
        }
        
        public virtual bool Delete(int id)
        {
            T entity = this.FindById(id);
            if (entity != null)
            {
                localLogger.LogInformation($"Found Entity, deleting : {id}");
                localRepository.Delete(entity);
                localRepository.Commit();
                return true;
            }            
            localLogger.LogVerbose($"Entity not found : {id}");            
            return false;
        }
    }
}