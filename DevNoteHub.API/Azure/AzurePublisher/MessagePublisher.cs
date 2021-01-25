using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using QR.API.Mapper;

namespace ServiceBus.Producer.Services
{
    public class MessagePublisher 
    {
        private readonly ITopicClient _topicClient;
        public MessagePublisher(string jsonPathConfig)
        {
            var setting =HTTPClientQR.LoadJson<JsonSettingQr>(jsonPathConfig);

            var topicClient = new TopicClient(setting.ServiceBusConnectionString
                ,setting.EntityPath);

            _topicClient = topicClient;
        }

        public MessagePublisher(string SvcBusConnString, string EntityPathTopic)
        {

            var topicClient = new TopicClient(SvcBusConnString,EntityPathTopic);

            _topicClient = topicClient;
        }


        public Task Publish<T>(T obj)
        {
            var objAsText = JsonConvert.SerializeObject(obj);
            var message = new Message(Encoding.UTF8.GetBytes(objAsText));
            message.UserProperties["messageType"] = typeof(T).Name;
            return _topicClient.SendAsync(message);
        }

        public async Task<bool> Send(Message msg)
        {               
            await  _topicClient.SendAsync(msg);
            return true;
        }


        public Task Publish(string raw)
        {
            var message = new Message(Encoding.UTF8.GetBytes(raw));
            message.UserProperties["messageType"] = "Raw";
            return _topicClient.SendAsync(message);
        }
    }
}