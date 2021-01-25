using MediatR;
using Steeroid.Business.Machine;
using Steeroid.Models;
using Steeroid.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using Steeroid.Models.BaseInterfaces;
//using Common.API.EventHandler;
using Steeroid.Models.Remoting;
using Steeroid.Models.Helpers;
using Steeroid.Business.Recorodings.Query;
using Steeroid.Business.Services;
using Steeroid.Models.Enums;

namespace Steeroid.Business.Areas.Input.Commands
{
   public class AutomateCmd : IRequest<ServiceResponse<string>>         //filename of WFInput.json

    {
        public DevNoteCmdEvent CommandDetails { get; set; }
    }

    /// <summary>
    /// returns filename fullpah of wfinput.json
    /// </summary>
    public class AutomateCmdHandler : IRequestHandler<AutomateCmd
        //filename of WFInput.json
        , ServiceResponse<string>>
    {

        IRepository _dbAccess;
        BizInputManager _inputManager;

        public AutomateCmdHandler(IRepository dbaccess,BizInputManager inputManager)
        {
            _dbAccess = dbaccess;
            _inputManager = inputManager;
        }


        public async Task<ServiceResponse<string>> Handle(AutomateCmd request, CancellationToken cancellationToken)
        {

            var result = new ServiceResponse<string>();
            var model = string.Empty;

            try
            {

                //var cmdHandler = new DevNoteIntegrationEventHandler();
                //await cmdHandler.Handle(@event);
                model = await _inputManager.CreateInputFile(request.CommandDetails); //Handle(request.CommandDetails);

            }
            catch (Exception err)
            {

                result.AddValidationMessage(err.Message);
            }

            result.AddModel(model);
            return result;
        }

        //public async Task<string> Handle(DevNoteCmdEvent @event)
        //{


        //    var result = string.Empty;
        //    try
        //    {

        //        //TIP# Configure AutoMapper
        //        var config = new MapperConfiguration(cfg => cfg.CreateMap<DevNoteCmdEvent
        //                    , RunWFCmdParam>());

        //        var mapper = config.CreateMapper();
        //        // Perform mapping
        //        RunWFCmdParam cmd = mapper.Map<DevNoteCmdEvent, RunWFCmdParam>(@event);

        //        cmd.EventName = @event.EventName;
        //        cmd.EventParameters = @event.EventParameters;
        //        cmd.GuidId = @event.GuidId;
        //        cmd.MessageId = @event.MessageId;
        //        cmd.Label = @event.Label;
        //        cmd.ReferenceId = @event.ReferenceId;
        //        cmd.GlobalMachineId = @event.GlobalMachineId;

        //        //_HACK safe to delete 
        //        #region---TEST ONLY: Compiler will  automatically erase this in RELEASE mode and it will not run if Global.GlobalTestMode is not set to TestMode.Simulation
        //        //#if DEBUG
        //        //                System.Diagnostics.Debug.WriteLine("HACK-TEST -DevNoteIntegrationEvent");
        //        //                if (GlobalDef.DEBUG_MODE == EnumDEBUG_MODE.TEST)
        //        //                {
        //        //                    var stringContent = JsonConvert.SerializeObject(@event);
        //        //                    Console.WriteLine("Azure recievedt: " + stringContent);

        //        //                    return;
        //        //                }


        //        //#endif
        //        #endregion //////////////END TEST

        //        if (!string.IsNullOrEmpty(@event.OuputResponse))
        //        {
        //            //do not trigger wf
        //            //STEP_.RESULT Confirmed from Azure
        //            // BotHttpClient.Log("Confirmed to AZURE: " + @event.OuputResponse);
        //            Agent.LogInfo("Confirmed to AZURE: " + @event.OuputResponse);

        //        }
        //        else
        //        {
        //            //STEP_.RECIEVER #304 create file
        //            //get record file

        //            /// return RegisterToken(enteredToken);
        //            SearchRecordCmd searchCmd = new SearchRecordCmd
        //            {
        //                EventName = @event.EventName
        //            };

        //            var response = await BizService.Mediator.Send(searchCmd);


        //            if (!response.HasError)
        //            {
        //                var record = response.Model;

        //                //CodeceptCmdParam
        //                //STEP_.EVENT CreateEventInput FileEnpoint here  
        //                cmd.JSFullPath = record.SourcePath;


        //                result =  await FileEndPointManager.CreateEventInput(cmd);

                      
        //            }


        //            else
        //            {
        //                Agent.LogError("No record found for event " + cmd.EventName);
        //                cmd.JSFullPath = string.Empty;
        //                cmd.ErrorCode = ErrorCodes.FileNotFound.ToString(); //"404";//ErrorCodes.FileNotFound;
        //                result = await FileEndPointManager.CreateEventInput(cmd);

        //            }


        //        }


        //    }
        //    catch (Exception err)
        //    {

        //        Agent.LogError(err.Message);
        //    }

        //    return result;

        //}


    }



}
