using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Steeroid.Business.Areas.Communication;
using Steeroid.Business.Machine;
using Steeroid.Business.Services;
//using Steeroid.Infra2._0.Areas.Communication;

namespace Steeroid.Infra2._0.Areas.Communication
{
    public class MyBusServiceManager : BizBusServiceManager
    {
        public MyBusServiceManager(IMachineManager repo):
             base(repo)
        {
           
        }

        public override void Step2_CreateMessageHandler()
        {

            BizService.MyAzure = new MyQueueClient(ThisMachine);
            //BizService.MyAzure.CreateMe(ThisMachine);
          

            var options = new MessageHandlerOptions((args) =>
            {
                Console.WriteLine($"Message handler encountered an exception {args.Exception}.");
                var context = args.ExceptionReceivedContext;
                Console.WriteLine("Exception context for troubleshooting:");
                Console.WriteLine($"- Endpoint: {context.Endpoint}");
                Console.WriteLine($"- Entity Path: {context.EntityPath}");
                Console.WriteLine($"- Executing Action: {context.Action}");

                if (args.Exception.HResult == -2146233088)
                {
                    //MyAzure.CloseAsync();

                }

                return Task.CompletedTask;
            });

            options.MaxConcurrentCalls = 10;
            options.AutoComplete = false;


            // STEP_.RECIEVER AZURE REcieve EVENT DevNoteServiceBusMessageHandler DevNoteServiceBusMessageHandler
            var client = (MyQueueClient)BizService.MyAzure;
            var serviceBusHandler = new DevNoteServiceBusMessageHandler(client.MyAzureClient);

            client.MyAzureClient.RegisterMessageHandler(serviceBusHandler.MessageReceivedHandler, options);

            
          
        }
    }
}
