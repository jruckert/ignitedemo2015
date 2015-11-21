namespace Ignite.Repositories
{
    using Entities;
    using Infrastructure;
    using Microsoft.Data.Entity;
    using Microsoft.Data.Entity.Infrastructure;
    using Microsoft.Data.Entity.Metadata;
    using System;

    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DatabaseContext(IServiceProvider serviceProvider, DbContextOptions options)
               : base(serviceProvider, options)
        {

        }

        public DbSet<TicketStatus> TicketStatus { get; set; }

        public DbSet<Ticket> Ticket { get; set; }

        public DbSet<Customer> Customer { get; set; }

        public void Commit()
        {
            SaveChanges();
        }

        public async void CommitAsync()
        {
            await SaveChangesAsync();
        }

        public DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class
        {
            return Set<TEntity>();
        }

        public bool IsDetachedEntityState(object entity)
        {
            return Entry(entity).State == EntityState.Detached;
        }

        public bool IsNewEntity(object entity)
        {
            return Entry(entity).State == EntityState.Added;
        }

        public void Reload(object entity)
        {
            throw new NotImplementedException();
        }

        public void SetModifiedEntityState(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
           builder.Entity<Customer>(b =>
            {
                b.HasMany(c => c.Tickets)
                    .WithOne(c => c.Customer)
                    .HasForeignKey(c => c.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Ticket>(b =>
            {
                b.HasOne(c => c.Status)
                    .WithMany()
                    .HasForeignKey(c => c.TicketStatusId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();
            });
        }
    }
}
