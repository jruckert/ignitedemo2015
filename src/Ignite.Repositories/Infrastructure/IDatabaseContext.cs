namespace Ignite.Repositories.Infrastructure
{
    using Entities;
    using Microsoft.Data.Entity;
    using System;

    public interface IDatabaseContext : IDisposable
    {
        DbSet<TicketStatus> TicketStatus { get; set; }

        DbSet<Ticket> Ticket { get; set; }

        DbSet<Customer> Customer { get; set; }

        DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class;

        void SetModifiedEntityState(object entity);

        bool IsDetachedEntityState(object entity);

        bool IsNewEntity(object entity);

        void Reload(object entity);

        void Commit();

        void CommitAsync();
    }
}