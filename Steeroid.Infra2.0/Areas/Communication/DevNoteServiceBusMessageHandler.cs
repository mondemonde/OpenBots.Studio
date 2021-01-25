using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using Common.API.EventHandler;
using MediatR;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using Steeroid.Business.Areas.Input.Commands;
using Steeroid.Business.Services;
using Steeroid.Infra2._0.DAL;
using Steeroid.Models;
using Steeroid.Models.BaseInterfaces;
using Steeroid.Models.Enums;
using Steeroid.Models.Helpers;

namespace Steeroid.Infra2._0.Areas.Communication
{
    public class DevNoteServiceBusMessageHandler 
    {
        //private readonly ISubscriptionClient client;
        private readonly QueueClient client;

        private readonly IMediator _mediator;

        public DevNoteServiceBusMessageHandler(QueueClient client)
        {
            this.client = client;
            _mediator =BizService.Resolve<IMediator>();



            // azuBOT = bot;
        }

        //MacDBContext _myDb;

        //public MacDBContext MyDb
        //{
        //    get
        //    {
        //        if (_myDb == null)
        //            _myDb = new MacDBContext();
        //        return _myDb;
        //    }
        //}
        public bool HasDuplicate(string messageId)
        {
            bool result = false;

            try
            {

                using (var MyDb = new MyDbContext())
                {
                    var myList = MyDb.BusMessages.OrderByDescending(c => c.Id).ToList();
                    result = myList.Where(m => m.MessageId == messageId).Count() > 0;

                    //clear morethan 100
                    for (int i = 0; i < myList.Count; i++)
                    {
                        if (i > 50)
                        {
                            MyDb.BusMessages.Remove(myList[i]);
                        }
                    }
                    MyDb.SaveChanges();
                }



            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);

            }
            return result;


        }

        //STEP.AZURE MessageReceivedHandler

        public async Task<ServiceResponse<bool>> MessageReceivedHandler(Message message, CancellationToken token)
        {

            ServiceResponse<bool> result = new ServiceResponse<bool>();
            var model = false;

            //_HACK safe to delete 
            #region---TEST ONLY: Compiler will  automatically erase this in RELEASE mode and it will not run if Global.GlobalTestMode is not set to TestMode.Simulation
#if OVERRIDE || OFFLINE

            System.Diagnostics.Debug.WriteLine("HACK-TEST -");
            // serialize JSON to a string and then write string to a file
            //File.WriteAllText(@"c:\movie.json", JsonConvert.SerializeObject(movie));

            // serialize JSON directly to a file
            using (StreamWriter file = File.CreateText(@"C:\_Dump\message.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, message);
            }

#endif
            #endregion //////////////END TEST






            var handler = new GenericAzureDTOHandler();

            //check for duplicate from bus message
            MessageManager msgManager = new MessageManager();
            // if (msgManager.HasDuplicate(message.MessageId))
            if (HasDuplicate(message.MessageId))
            {
                message.Label = "DUPLICATE-" + message.Label;
                await handler.Handle(message);
                result.AddModel(model);

                result.AddValidationMessage(message.Label);
                return result;

            }
            else
            {
                //log
                await handler.Handle(message);
                //and continue
            }

            //STEP_.RECIEVER #502 MessageReceivedHandler
            //Console.WriteLine($"Received message1: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");
            var mode = EnumSynch.NONE;
            try
            {
                var objMode = message.UserProperties["Mode"];
                //EnumSynch mode = EnumSynch @event.Mode
                Enum.TryParse(objMode.ToString(), out EnumSynch thisMode);
                mode = thisMode;

            }
            catch (Exception)
            {

            }


            switch (mode)
            {
                case EnumSynch.NONE:


                    //_HACK safe to delete 

                    Agent.LogTest("NONE");
                    model = true;
                    break;
                case EnumSynch.DEVHUB_UPDATE:
                    model = true;
                    break;
                case EnumSynch.DEVHUB_DELETE:
                    model = true;
                    break;
                case EnumSynch.DEVHUB_INSERT:
                    model = true;
                    break;
                case EnumSynch.DEVFRONT_RUN:
                {

                    //STEP_.RECIEVER #2 MessageReceivedHandler
                    Console.WriteLine($"Received message1: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");

                    var jsonMessage = Encoding.UTF8.GetString(message.Body);
                    var @event = JsonConvert.DeserializeObject<DevNoteCmdEvent>(jsonMessage);

                    //message.UserProperties["GlobalMachineId"] = mac.Id.ToString();
                    //message.UserProperties["DomainName"] = mac.DomainName;
                    //message.UserProperties["Topic"] = mac.TopicId;


                    @event.EventName = message.Label;
                    @event.GuidId = message.MessageId;
                    @event.ReferenceId = message.UserProperties["ReferenceId"].ToString();

                    @event.GlobalMachineId = Convert.ToInt32(message.UserProperties["GlobalMachineId"]);
                    @event.Topic = message.UserProperties["Topic"].ToString();

                    @event.DomainName = message.UserProperties["DomainName"].ToString();

                    //@event.ReferenceId = message.UserProperties["ReferenceId"].ToString();


                    @event.Mode = mode.ToString();

                    //after building DevNoteCmdEvent fire mediator
                    AutomateCmd thisCmd = new AutomateCmd
                    {
                        CommandDetails = @event
                    };

                    var cmdResult = await _mediator.Send(thisCmd);

                    //var cmdHandler = new DevNoteIntegrationEventHandler();
                    //await cmdHandler.Handle(@event);
                    if(cmdResult.HasError)
                    {
                        result.AddValidationMessage(cmdResult.ErrorMessages.FirstOrDefault());
                    }
                    else
                    {
                        model = true;
                    }


                    Agent.LogTest("DEVFRONT_RUN");
                    break;
                }

                default:
                    break;
            }


            if (mode != EnumSynch.NONE)
            {
                ////1.get message machine id
                //var globalMacId = Convert.ToInt32(message.UserProperties["GlobalMachineId"]);
                ////2.get private key
                //MachineManager mngr = new MachineManager();
                //var mac = mngr.GetById(globalMacId);

                ////3. decrypt
                //var jsonMessage = Encoding.UTF8.GetString(message.Body);

                //var @decrypted = CryptoEngine.DecryptWithPrivateKey(jsonMessage, mac.PrivateKey);

                //var @event = JsonConvert.DeserializeObject<EventDTO>(@decrypted);
                //var handler = new DevNoteEventDTOHandler();
                //await handler.Handle(@event);

            }
            else
            {
                //do nothing

            }


            try
            {

              if(!Agent.EnableTestLog)
              await client.CompleteAsync(message.SystemProperties.LockToken);

            }
            catch (Exception err)
            {

                model = false;
                result.AddValidationMessage(err.Message);
            }

            result.AddModel(model);
            return result;


        }



    }

}
