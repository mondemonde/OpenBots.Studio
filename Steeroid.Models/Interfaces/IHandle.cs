using System.Threading.Tasks;

namespace Steeroid.Models.Interfaces
{
    public interface IHandle<in T> where T : BaseDomainEvent
    {
        Task Handle(T domainEvent);
    }
}