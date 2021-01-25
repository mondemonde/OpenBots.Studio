using System.Threading.Tasks;

namespace Steeroid.Models.Interfaces
{
    public interface IDomainEventDispatcher
    {
        Task Dispatch(BaseDomainEvent domainEvent);
    }

}