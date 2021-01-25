using Microsoft.Azure.ServiceBus;
using System.Threading.Tasks;

namespace ServiceBus.Producer.Services
{
    public interface IQueuePublisher
    {
        Task Publish(string raw);
        Task Publish<T>(T obj);
        Task<bool> Send(Message msg);
    }
}