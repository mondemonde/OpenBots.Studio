using DevNote.Interface;
using IntegrationEvents.Events.DevNote;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EFCoreTransactionsReceiver
{
    public class DevNoteServiceBusMessageHandler
    {
        private readonly ISubscriptionClient client;
        private readonly string connectionString;

        //public IBotHost azuBOT { get; set; }


        public DevNoteServiceBusMessageHandler(ISubscriptionClient client)
        {
            this.client = client;
           // azuBOT = bot;
        }

        //STEP.AZURE MessageReceivedHandler
        
        public async Task MessageReceivedHandler(Message message, CancellationToken token)
        {
            //STEP_.RECIEVER #2 MessageReceivedHandler
            Console.WriteLine($"Received message1: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");

            var jsonMessage = Encoding.UTF8.GetString(message.Body);

            //step# 40.1  case "DevNoteIntegrationEvent":
            //switch (message.Label)
            //{
            //    case "DevNoteIntegrationEvent":
            //        var @event = JsonConvert.DeserializeObject<DevNoteIntegrationEvent>(jsonMessage);

            //        var handler = new DevNoteIntegrationEventHandler();                  
            //        await handler.Handle(@event);                 


            //        break;
            //    default:
            //        Console.WriteLine(jsonMessage);
            //        break;
            //}

            var @event = JsonConvert.DeserializeObject<DevNoteIntegrationEvent>(jsonMessage);
            var handler = new DevNoteIntegrationEventHandler();

            await handler.Handle(@event);

            await client.CompleteAsync(message.SystemProperties.LockToken);
        }



    }
}

