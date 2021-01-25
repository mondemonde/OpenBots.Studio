using DevNote.Interface.Models;
using DevNoteHub.API.Encryption;
using DevNoteHub.API.Interface;
using DevNoteHub.API.Models;
using DevNoteHub.Models;
using IntegrationEvents.Events.DevNote;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using QR.API.Mapper.Std;
using ServiceBus.Producer.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DevNoteHub.API.Service
{
   public class SynchManager
    {


        //public SubscriptionClient MyAzure { get; set; }
        QueuePublisher _myPublisher;

        IMacManager _mngr;


       public SynchManager(IMacManager mngr)
        {
            _mngr = mngr;
        }

        public QueuePublisher MyPublisher {
            get
            {
                if(_myPublisher==null)
                {
                    CreatePublisher(_mngr);
                }

                return _myPublisher;
            }
        }

         void CreatePublisher(IMacManager mngr)
        {


            //1.get encrypted subcriiptiion
            var mac =mngr.GetMachineRecord();

            //2.decypt
            //var connString = mac.ServiceBusConnectionString;//DecryptSvcBusConnString(mac);
            //var topic = mac.TopicId;// DecryptSvcBusEntityPathTopic(mac);


            var connString = ConfigurationManager.AppSettings["DevnoteHubServiceBus"] ?? "";
            var topic = ConfigurationManager.AppSettings["DevNoteHubQueueName"] ?? "";


            //_myPublisher = new MessagePublisher(connString, topic);
            _myPublisher = new QueuePublisher(connString, topic);




        }


        public 

        string CreateMessageBody(WFProfile eventRec,EnumSynch mode,ref EventDTO mBody)
        {



            // EventDTO mBody = new EventDTO();

            mBody.EventName = mode.ToString();
            var mac = _mngr.GetMachineRecord();

            mBody.GlobalMachineId = mac.GlobalMacId;
            mBody.Event = _mngr.GetEventToUpdateFromFront(eventRec.Id);
            mBody.Params = _mngr.GetEventParamsToUpdateFromFront(eventRec.Id);

            mBody.Event.WFProfileId = mBody.Event.Id;


            var jsonMessage = JsonConvert.SerializeObject(mBody);

            //encrypt..
            //STEP_.HUB #2 encypt body
            //return jsonMessage;//CryptoEngine.Encrypt(jsonMessage,key;
            // return CryptoEngine.EncryptWithPrivateKey(jsonMessage, mac.PrivateKey);
            return jsonMessage;


        }



        //front to send update
        public async Task<bool> SendUpdateToHub(WFProfile eventRec,EnumSynch mode)
        {
            try
            {
                var thisMac = _mngr.GetMachineRecord();


                EventDTO @event = new EventDTO();
                @event.EventName = mode.ToString();
                @event.Mode = mode.ToString();

                @event.Topic = "Mac#" + thisMac.GlobalMacId.ToString() + "-" + eventRec.Domain;
                var encrypted = CreateMessageBody(eventRec,mode,ref @event);
                var messageBody = Encoding.UTF8.GetBytes(encrypted);



                //STEP_.DevHub #3 Create Message
                var message = new Message
                {
                    
                    MessageId =@event.Topic,
                    Body = messageBody,
                    Label = @event.EventName //"XamunLeave"//devEvent.EventName
                };

                //Security part...
                message.UserProperties["Domain"] = eventRec.Domain;
                //compute the machine Id here..               
                message.UserProperties["GlobalMachineId"] =thisMac.GlobalMacId.ToString();
                // Console.WriteLine($"Send message: Body:{Encoding.UTF8.GetString(message.Body)}");

                message.UserProperties["Mode"] = mode.ToString();



                await MyPublisher.Send(message);

                return true;



            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                //return Request.CreateResponse(HttpStatusCode.BadRequest, new string[] { err.Message });
                return false;


            }

        }

        public async Task<bool> SendOutputToHub(DevNoteIntegrationEvent eventRec)
        {
            EnumSynch mode = EnumSynch.DEVHUB_SAVE_OUTPUT;
            try
            {
                var thisMac = _mngr.GetMachineRecord();


                EventDTO @event = new EventDTO();
                @event.EventName = mode.ToString();
                @event.Mode = mode.ToString();

                @event.Topic = "Mac#" + thisMac.GlobalMacId.ToString() + "-" + eventRec.DomainName;

                var jsonMessage = JsonConvert.SerializeObject(eventRec);
               
                var messageBody = Encoding.UTF8.GetBytes(jsonMessage);



                //STEP_.DevHub #3 Create Message
                var message = new Message
                {

                    MessageId = @event.Topic,
                    Body = messageBody,
                    Label = @event.EventName //"XamunLeave"//devEvent.EventName
                };

                //Security part...
                message.UserProperties["Domain"] = eventRec.DomainName;
                //compute the machine Id here..               
                message.UserProperties["GlobalMachineId"] = thisMac.GlobalMacId.ToString();
                // Console.WriteLine($"Send message: Body:{Encoding.UTF8.GetString(message.Body)}");

                message.UserProperties["Mode"] = mode.ToString();



                await MyPublisher.Send(message);

                return true;



            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                //return Request.CreateResponse(HttpStatusCode.BadRequest, new string[] { err.Message });
                return false;


            }

        }


        public void PickUpdate()
        {

        }

        public string DecryptSvcBusConnString(IMacManager mngr)
        {
            string result = string.Empty;

            var mac = mngr.GetMachineRecord();
            if (mac.Id > 1)
            {
                result = CryptoEngine.DecryptWithPrivateKey(mac.ServiceBusConnectionString, JwtAuthManager.GetDefaultLocalKey()) ?? string.Empty;
            }
            else
                result = mac.ServiceBusConnectionString;
            return result;
        }
        public string DecryptSvcBusConnString(MachineServer mac)
        {
            string result = string.Empty;

            if (mac.Id > 1)
            {

                result = CryptoEngine.DecryptWithPrivateKey(mac.ServiceBusConnectionString, JwtAuthManager.GetDefaultLocalKey()) ?? string.Empty;
            }
            else
                result = mac.ServiceBusConnectionString;

            return result;
        }


        public string DecryptSvcBusEntityPathTopic(IMacManager mngr)
        {
            string result = string.Empty;
            var mac = mngr.GetMachineRecord();

            if (mac.Id > 1)
            {
                result = CryptoEngine.DecryptWithPrivateKey(mac.TopicId, JwtAuthManager.GetDefaultLocalKey()) ?? string.Empty;
            }
            else
                result = mac.TopicId;

            return result;
        }
        public string DecryptSvcBusEntityPathTopic(MachineServer mac)
        {
            string result = string.Empty;

            if (mac.Id > 1)
            {

                result = CryptoEngine.DecryptWithPrivateKey(mac.TopicId,
                    JwtAuthManager.GetDefaultLocalKey()) ?? string.Empty;
            }
            else
                result = mac.TopicId;

            return result;
        }

        public MachineServer GetMyGlobalMachineId()
        {
           return _mngr.GetMachineRecord();
        }

    }
}
