using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Steeroid.Business.Areas.Communication;
using Steeroid.Models;
using Steeroid.Models.BaseInterfaces;
using Steeroid.Models.Helpers;

namespace Steeroid.Infra2._0.Areas.Communication
{
    public class MyQueueClient : IBizQueueClient
    {
        MachineServer _thisMac;
        public MyQueueClient(MachineServer thisMac)
        {
            CreateMe(thisMac);
            _thisMac = thisMac;
        }



        public QueueClient MyAzureClient { get; set; }

        public IBizQueueClient CreateMe(MachineServer thisMac)
        {
           MyAzureClient =  new QueueClient(thisMac.ServiceBusConnectionString
                 , thisMac.TopicId);

            return this;
        }

        public async Task<ServiceResponse<bool>> SendMessage(string body)
        {
            ServiceResponse<bool> result = new ServiceResponse<bool>();
            bool model = true;

            try
            {

                QueueClient queueClient = new QueueClient(_thisMac.ServiceBusConnectionString
                 , _thisMac.TopicId, ReceiveMode.ReceiveAndDelete);//(svc, queueName, ReceiveMode.ReceiveAndDelete);


                // Create a new message to send to the queue.
                string messageBody = body;//$"Message check test.";
                var message = new Microsoft.Azure.ServiceBus.Message(Encoding.UTF8.GetBytes(messageBody));
                // Write the body of the message to the console.
                Console.WriteLine($"Sending message: {messageBody}");
                // Send the message to the queue.
                await queueClient.SendAsync(message);
                await queueClient.CloseAsync();
               
            }

            catch (Exception exception)
            {
                model = false;
                result.AddValidationMessage(exception.Message);
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
                Agent.LogError(exception);
                
            }

            result.AddModel(model);
            return result;
        }
    }
}
