using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace ServiceBus.Producer.Services
{
    public class QueuePublisher 
    {
        private readonly IQueueClient _qClient;

        public QueuePublisher(string svcBusConnString, string queueName)
        {

            var qClient = new QueueClient(svcBusConnString, queueName);

            _qClient = qClient;
        }


        public Task Publish<T>(T obj)
        {
            var objAsText = JsonConvert.SerializeObject(obj);
            var message = new Message(Encoding.UTF8.GetBytes(objAsText));
            message.UserProperties["messageType"] = typeof(T).Name;
            return _qClient.SendAsync(message);
        }

        public async Task<bool> Send(Message msg)
        {
            await _qClient.SendAsync(msg);
            return true;
        }


        public Task Publish(string raw)
        {
            var message = new Message(Encoding.UTF8.GetBytes(raw));
            message.UserProperties["messageType"] = "Raw";
            return _qClient.SendAsync(message);
        }
    }
}