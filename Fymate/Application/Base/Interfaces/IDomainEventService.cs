using Fymate.Domain.Common;
using System.Threading.Tasks;

namespace Fymate.Core.Base.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
