using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.ServiceBus;

namespace SteeroidXUnitTest.Areas.Communication
{
   public static class MessageFactory
    {
       public static Message DefaultMessage
        {
            get
            {
               

                var msg = new Message
                {
                    //ContentType = null,
                    //CorrelationId = null,
                    //ExpiresAtUtc = DateTime.Now.AddDays(1),
                    Label = "testvar",
                    MessageId = "8ce1ec61-ac42-4632-813a-8217d4f53bb1",
                    //PartitionKey = null,
                    //ReplyTo = null,
                    //ReplyToSessionId = null,
                    ScheduledEnqueueTimeUtc = DateTime.Now.AddMinutes(10),
                    //SessionId = null,
                    //Size= 106,
                    //SystemProperties: { Microsoft.Azure.ServiceBus.Message.SystemPropertiesCollection}
                    TimeToLive = new TimeSpan(1400,00,00),
                    //To= null,
                    //UserProperties: Count = 9
                    //ViaPartitionKey: null
                    //messageId= "8ce1ec61-ac42-4632-813a-8217d4f53bb1"
                    //partitionKey: null
                    //replyToSessionId: null
                    //sessionId: null
                    //timeToLive: { 14.00:00:00}
                               // viaPartitionKey: null

                };

                msg.UserProperties[]

            }
        }
    }
}
