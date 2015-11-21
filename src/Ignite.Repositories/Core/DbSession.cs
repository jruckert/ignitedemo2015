namespace Ignite.Repositories.Core
{
    using Infrastructure;
    using Microsoft.Data.Entity;
    using Microsoft.Data.Entity.ChangeTracking;
    using Microsoft.Data.Entity.Storage;
    using System.Threading.Tasks;

    public class DbSession : IDbSession
    {
        private readonly IDatabaseContext currentContext;
        public DbSession(IDatabaseContext context)
        {
            this.currentContext = context;
        }

        public IDatabaseContext Current
        {
            get
            {
                return this.currentContext;
            }
        }

        public ChangeTracker TrackChanges()
        {
            return ((DbContext)Current).ChangeTracker;
        }

        public IDbContextTransaction BeginTransaction()
        {
            return ((DbContext)Current).Database.BeginTransaction();
        }

        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return ((DbContext)Current).Database.BeginTransactionAsync();
        }

        public int SaveChanges()
        {
            return ((DbContext)Current).SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return ((DbContext)Current).SaveChangesAsync();
        }        
    }
}
