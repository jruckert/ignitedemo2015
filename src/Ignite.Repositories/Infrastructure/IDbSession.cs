namespace Ignite.Repositories.Infrastructure
{
    using Microsoft.Data.Entity.ChangeTracking;
    using Microsoft.Data.Entity.Storage;
    using System.Threading.Tasks;

    public interface IDbSession
    {
        IDatabaseContext Current { get; }

        ChangeTracker TrackChanges();

        IDbContextTransaction BeginTransaction();

        Task<IDbContextTransaction> BeginTransactionAsync();

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}