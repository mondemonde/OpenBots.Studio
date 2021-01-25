//using SteeroidUpdate.Main.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using JetBrains.Annotations;
using Steeroid.Models;
using Steeroid.Models.Interfaces;

namespace SteeroidUpdate.Main.Infrastructure.Data
{
    public class MachineDbContext : DbContext
    {
        private readonly IDomainEventDispatcher _dispatcher;


        //public AppDbContext(DbContextOptions options) : base(options)
        //{
        //}

        public MachineDbContext(DbContextOptions<MachineDbContext> options, IDomainEventDispatcher dispatcher)
            : base(options)
        {
            
            _dispatcher = dispatcher;
        }

        //public BooksContext(string connectionString) : base(GetOptions(connectionString))
        //{
        //}

        //private static DbContextOptions GetOptions(string connectionString)
        //{
        //    return SqliteDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        //}

       

        public DbSet<MachineServer> MachineServers { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.ApplyAllConfigurationsFromCurrentAssembly();

            // alternately this is built-in to EF Core 2.2
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // ignore events if no dispatcher provided
            if (_dispatcher == null) return result;

            // dispatch events only if save was successful
            var entitiesWithEvents = ChangeTracker.Entries<BaseModel>()
                .Select(e => e.Entity)
                .Where(e => e.Events.Any())
                .ToArray();

            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.Events.ToArray();
                entity.Events.Clear();
                foreach (var domainEvent in events)
                {
                    await _dispatcher.Dispatch(domainEvent).ConfigureAwait(false);
                }
            }

            return result;
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}