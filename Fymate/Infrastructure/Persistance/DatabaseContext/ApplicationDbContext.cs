using Core.Base.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistance.DatabaseContext
{
    public class ApplicationDbContext: DbContext, IApplicationDbContext
    {
        private readonly IDomainEventService _domainEventService;
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Advert> Adverts { get; set; }
        public ApplicationDbContext(DbContextOptions options, IDomainEventService domainEventService) : base(options)
        {
            _domainEventService = domainEventService;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Seed();
        }
    }
}
