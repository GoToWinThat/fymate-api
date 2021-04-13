using Fymate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Fymate.Core.Base.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Advert> Adverts { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
