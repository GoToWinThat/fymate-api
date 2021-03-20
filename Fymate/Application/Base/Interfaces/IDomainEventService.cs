using Domain.Common;
using System.Threading.Tasks;

namespace Core.Base.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
