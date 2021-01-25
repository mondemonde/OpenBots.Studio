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
using Steeroid.Business.Areas.Bot;
using System.Net.Http;
using System.Net;

namespace Steeroid.Business.Areas.Input.Commands
{
   public class AutoRunFromFileCmd : IRequest<ServiceResponse<HttpResponseMessage>>         //filename of WFInput.json

    {
        public RunWFCmdParam Params { get; set; }
    }

    /// <summary>
    /// returns filename fullpah of wfinput.json
    /// </summary>
    public class AutoRunFromFileCmdHandler : IRequestHandler<AutoRunFromFileCmd
        //filename of WFInput.json
        , ServiceResponse<HttpResponseMessage>>
    {

        IRepository _dbAccess;
        //BizInputManager _inputManager;
        IOPenBot _botAgent;

        public AutoRunFromFileCmdHandler(IRepository dbaccess, IOPenBot bot)
        {
            _dbAccess = dbaccess;
           _botAgent = bot;
        }


        public async Task<ServiceResponse<HttpResponseMessage>> Handle(AutoRunFromFileCmd request, CancellationToken cancellationToken)
        {

            var result = new ServiceResponse<HttpResponseMessage>();

            //TIP new HttpResponseMessage
            var model= new HttpResponseMessage(HttpStatusCode.BadRequest)
            { Content = new StringContent("OpenBot agent API error.") };
        
            try
            {

                //var cmdHandler = new DevNoteIntegrationEventHandler();
                //await cmdHandler.Handle(@event);
                var cmdResult = await _botAgent.HandleCmd(request.Params);

                model = cmdResult;

                //_HACK safe to delete 
         #region---TEST ONLY: Compiler will  automatically erase this in RELEASE mode and it will not run if Global.GlobalTestMode is not set to TestMode.Simulation
            Agent.LogTest("AutoRunCmd");         
        #endregion //////////////END TEST



            }
            catch (Exception err)
            {

                result.AddValidationMessage(err.Message);
            }

            result.AddModel(model);
            return result;
        }



    }



}
