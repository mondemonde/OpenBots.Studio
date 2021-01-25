
using AutoMapper;
using MediatR;
//using Common.COMMANDS;
//using IntegrationEvents.Events.DevNote;
using Newtonsoft.Json;
using Steeroid.Business.Areas.Communication.WebApi;
using Steeroid.Business.Recorodings.Query;
using Steeroid.Business.Services;
using Steeroid.Models;
using Steeroid.Models.Enums;
using Steeroid.Models.Helpers;
using Steeroid.Models.Remoting;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;

namespace Common.API.EventHandler
{
    public class DevNoteIntegrationEventHandler// : IIntegrationEventHandler<DevNoteCmdEvent>
    {
        private readonly IMediator _mediator;

        // public  IBotHost azuBOT { get; set; }

        //public DevNoteIntegrationEventHandler(IBotHost bot)
        //{
        //    azuBOT = bot;
        //}

        public DevNoteIntegrationEventHandler()
        {
           _mediator = BizService.Resolve<IMediator>();
        }



        //STEP.Receiver #541 Handle(DevNoteIntegrationEvent @event)
        public async Task Handle(DevNoteCmdEvent @event)
        {
            try
            {
              
                //TIP# Configure AutoMapper
                var config = new MapperConfiguration(cfg => cfg.CreateMap<DevNoteCmdEvent
                            , RunWFCmdParam>());

                var mapper = config.CreateMapper();
                // Perform mapping
                RunWFCmdParam cmd = mapper.Map<DevNoteCmdEvent, RunWFCmdParam>(@event);

                cmd.EventName = @event.EventName;
                cmd.EventParameters = @event.EventParameters;
                cmd.GuidId = @event.GuidId;
                cmd.MessageId = @event.MessageId;
                cmd.Label = @event.Label;
                cmd.ReferenceId = @event.ReferenceId;
                cmd.GlobalMachineId = @event.GlobalMachineId;

                //_HACK safe to delete 
                #region---TEST ONLY: Compiler will  automatically erase this in RELEASE mode and it will not run if Global.GlobalTestMode is not set to TestMode.Simulation
                //#if DEBUG
                //                System.Diagnostics.Debug.WriteLine("HACK-TEST -DevNoteIntegrationEvent");
                //                if (GlobalDef.DEBUG_MODE == EnumDEBUG_MODE.TEST)
                //                {
                //                    var stringContent = JsonConvert.SerializeObject(@event);
                //                    Console.WriteLine("Azure recievedt: " + stringContent);

                //                    return;
                //                }


                //#endif
                #endregion //////////////END TEST

                if (!string.IsNullOrEmpty(@event.OuputResponse))
                {
                    //do not trigger wf
                    //STEP_.RESULT Confirmed from Azure
                    // BotHttpClient.Log("Confirmed to AZURE: " + @event.OuputResponse);
                    Agent.LogInfo("Confirmed to AZURE: " + @event.OuputResponse);

                }
                else
                {
                    //STEP_.RECIEVER #304 create file
                    //get record file

                    /// return RegisterToken(enteredToken);
                    SearchRecordCmd searchCmd = new SearchRecordCmd
                    {
                        EventName = @event.EventName
                    };

                    var response = await _mediator.Send(searchCmd);


                    if (!response.HasError)
                    {
                        var record = response.Model;

                        //CodeceptCmdParam
                        //STEP_.EVENT CreateEventInput FileEnpoint here  
                        cmd.JSFullPath = record.SourcePath;
                        await FileEndPointManager.CreateEventInput(cmd);

                        //STEP_.RECIEVER Let timer take one event At a time

                        //STEP_.RECIEVER this done now buy a timer...
                        //var carrierCmd = LogApplication.Common.Commands.CmdParam
                        //    .CreateCmdCarrier<RunWFCmdParam>(cmd);

                        //await BotHttpClient.PostToDevNote(carrierCmd);
                    }


                    else
                    {
                        Agent.LogError("No record found for event " + cmd.EventName);
                        cmd.JSFullPath = string.Empty;
                        cmd.ErrorCode = ErrorCodes.FileNotFound.ToString(); //"404";//ErrorCodes.FileNotFound;
                        await FileEndPointManager.CreateEventInput(cmd);

                    }


                }


            }
            catch (Exception err)
            {

               Agent.LogError(err.Message);
            }



        }

    }
}
