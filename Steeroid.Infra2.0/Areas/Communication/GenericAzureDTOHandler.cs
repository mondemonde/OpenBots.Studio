using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Steeroid.Models;

namespace Steeroid.Infra2._0.Areas.Communication
{
    public class GenericAzureDTOHandler
    {

        // public  IBotHost azuBOT { get; set; }

        //public DevNoteIntegrationEventHandler(IBotHost bot)
        //{
        //    azuBOT = bot;
        //}

        public GenericAzureDTOHandler()
        {

        }



        //STEP.Azure #41 Handle(DevNoteIntegrationEvent @event)
        public async Task Handle(Message message)
        {

            MessageManager MsgManager = new MessageManager();

            try
            {

                var jsonMessage = Encoding.UTF8.GetString(message.Body);
                //Log...

                BusMessage msg = new BusMessage
                {
                    MessageId = message.MessageId,
                    Topic = "MessageId: " + message.MessageId,
                    Label = message.Label,
                    Content = jsonMessage,
                    Created = DateTime.Now
                };


                MsgManager.MyDb.BusMessages.Add(msg);
                await MsgManager.MyDb.SaveChangesAsync();

            }
            catch (Exception err)
            {
                BusMessage msg = new BusMessage
                {
                    MessageId = message.MessageId,
                    Topic = "MessageId: " + message.MessageId,
                    Label = "ERROR on " + message.Label,
                    Content = err.Message,
                    Created = DateTime.Now
                };

                MsgManager.MyDb.BusMessages.Add(msg);
                await MsgManager.MyDb.SaveChangesAsync();
            }



        }



    }

}
