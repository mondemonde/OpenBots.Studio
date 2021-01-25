using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using MediatR;
using Steeroid.Business.Areas.Bot;
using Steeroid.Business.Areas.Output;
using Steeroid.Business.Recorodings.Commands;
using Steeroid.Business.Services;
using Steeroid.Infra2._0.Areas.Scripting;
using Steeroid.Infra2._0.DAL;
//using Steeroid.Infra2._0.Services;
using Steeroid.Models;
using Steeroid.Models.Common.Commands;
using Steeroid.Models.Enums;
using Steeroid.Models.Helpers;
using Steeroid.Models.Remoting;

namespace Steeroid.Infra2._0.Areas.Bot
{

    class MyOpenBot : IOPenBot
    {
        public CmdParam MyCmd { get; set; }
        public int RetryCount { get; set; }
        public string WFInputFilePath { get; set; }

        private IMediator _mediator;
       public  IMediator Mediator
        {
            get
            {
                if(_mediator==null)
                 _mediator = BizService.Resolve<IMediator>();

                return _mediator;
            }
        }


        // frmScriptBuilder MyBuilder;

        public MyOpenBot()
        {
          // _mediator = GlobalService.Factory.Resolve<IMediator>();
          //  _mediator = BizService.Resolve<IMediator>();

        }

        public async Task<HttpResponseMessage>  AutoRun(RunWFCmdParam cmd)
        {

            OBRunner runner = new OBRunner();

            string scriptFile = cmd.EventFilePath; // @"F:\Users\rgalvez\Documents\OpenBotsStudio\My Scripts\Demo1\Main.json";

            runner.RunAutomation(scriptFile);

            //var cond1 = new TaskWaiter.Conditions("wait_for_result.txt");
            //await cond1.WaitUntil(() => AutoPlayPolicy.AssertPlayerResultExist(started) == true, 1000).ContinueWith(x =>

            //next
            //?insert polly here..
            OutputManager.IsPollyDone =  await  CheckIsTaskDone();
      

            var cond = new WaitConditions("Autorun_"+ cmd.EventName);
            //await cond.WaitUntil(() => OutputManager.IsPollyDone && OutputManager.IsTasktDone);
            await cond.WaitUntil(() => OutputManager.IsPollyDone );



            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(cmd.EventName + " event is executed.") };




        }

     async    Task< bool> CheckIsTaskDone()
        {
            //next 
            //?implement api call or just check polly
            await Task.Delay(3000);

            return true;

        }

        public Task<HttpResponseMessage> AutoRunCodeCept(RunWFCmdParam cmd)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> AutoRunTest()
        {
            throw new NotImplementedException();
        }

        public async Task<HttpResponseMessage> HandleCmd(CmdParam param)
        {
            HttpResponseMessage result;
            MyCmd = param;

            if (param.CommandName == EnumCmd.DBGetEvents.ToString())
            {
                using (MyDbContext thisDb = new MyDbContext())
                {
                    var list = thisDb.WFProfiles.Where(p => p.InActive == false).ToList();
                    var resultList = list.OrderBy(p => p.Tag).Select(p => p.Tag).ToArray();


                    //TIP: CreateResponse(HttpStatusCode.OK, result);
                    var configuration = new HttpConfiguration();
                    HttpRequestMessage req = new HttpRequestMessage();
                    req.SetConfiguration(configuration);

                    result = req.CreateResponse(HttpStatusCode.OK, resultList);
                    //return new HttpResponseMessage(HttpStatusCode.OK) { Content = result }  };


                }

                //  return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(param.CommandName + "  is executed.") };

            }
            else if (param.CommandName == EnumCmd.DBGetParameters.ToString())
            {
                string eventTag = Convert.ToString(param.Payload);
                Dictionary<string, string> resultDic = new Dictionary<string, string>();
                using (MyDbContext thisDb = new MyDbContext())
                {
                    var profile = thisDb.WFProfiles.Where(p => p.Tag.ToLower() == eventTag.ToLower()).ToList().LastOrDefault();
                    if (profile != null && profile.Id > 0)
                    {

                        //STEP_.Player #807 get dbparameters
                        var list = thisDb.WFProfileParameters.Where(a => a.WFProfileId == profile.Id).ToList();
                        foreach (var arg in list)
                        {
                            //make lower case for propertyname
                            resultDic.Add(arg.MappedTo_Input_X, arg.PropertyName.ToLower());
                            //---------------------------------------(PropertyName, value); this is the external dictionary  crossed
                            //results to ---(arg.MappedTo_Input_X,value)

                        }

                    }

                }
                //TIP: CreateResponse(HttpStatusCode.OK, result);
                var configuration = new HttpConfiguration();
                HttpRequestMessage req = new HttpRequestMessage();
                req.SetConfiguration(configuration);
                result = req.CreateResponse(HttpStatusCode.OK, resultDic);
            }
            else if (param.CommandName == EnumCmd.TasktVars.ToString())
            {
                string eventTag = Convert.ToString(param.Payload);
                Dictionary<string, string> resultDic = new Dictionary<string, string>();
                using (MyDbContext thisDb = new MyDbContext())
                {
                    var profile = thisDb.WFProfiles.Where(p => p.Tag.ToLower() == eventTag.ToLower()).ToList().LastOrDefault();
                    if (profile != null && profile.Id > 0)
                    {

                        ScriptManager mngr = new ScriptManager();
                        mngr.ReadWriteXml(profile.SourcePath, resultDic);
                        resultDic = ScriptManager.TasktVars;

                        ////STEP_.Player #807 get dbparameters
                        //var list = thisDb.WFProfileParameters.Where(a => a.WFProfileId == profile.Id).ToList();
                        //foreach (var arg in list)
                        //{
                        //    //make lower case for propertyname
                        //    resultDic.Add(arg.MappedTo_Input_X, arg.PropertyName.ToLower());
                        //    //results to ---(arg.MappedTo_Input_X,value)
                        //}
                    }

                }
                //TIP: CreateResponse(HttpStatusCode.OK, result);
                var configuration = new HttpConfiguration();
                HttpRequestMessage req = new HttpRequestMessage();
                req.SetConfiguration(configuration);
                result = req.CreateResponse(HttpStatusCode.OK, resultDic);
            }
            else if (param.CommandName == EnumCmd.TasktVarsUpdate.ToString())
            {

                var current = (WFProfile)(param.Payload);
                var cmd = new SaveRecordCmd
                {
                    Event = current
                };

                var response = await Mediator.Send(cmd);
                var model = response.Model;

                if (response.HasError)
                {
                    var configuration = new HttpConfiguration();
                    HttpRequestMessage req = new HttpRequestMessage();
                    req.SetConfiguration(configuration);
                    return req.CreateResponse(HttpStatusCode.BadRequest, model);

                }
                else
                {
                    //TIP: CreateResponse(HttpStatusCode.OK, result);
                    var configuration = new HttpConfiguration();
                    HttpRequestMessage req = new HttpRequestMessage();
                    req.SetConfiguration(configuration);
                    return req.CreateResponse(HttpStatusCode.OK, model);

                }





                //TIP: CreateResponse(HttpStatusCode.OK, result);
                //var configuration = new HttpConfiguration();
                //HttpRequestMessage req = new HttpRequestMessage();
                //req.SetConfiguration(configuration);
                //return req.CreateResponse(HttpStatusCode.OK, resultDic);
            }
            else if (param.CommandName == EnumCmd.TasktVarsOpen.ToString())
            {
                int thisId = Convert.ToInt32(param.Payload);
                using (MyDbContext thisDb = new MyDbContext())
                {
                    var profile = thisDb.WFProfiles.First(p => p.Id == thisId);
                    if (profile != null && profile.Id > 0)
                    {
                        //var asyncResult = AsyncHelper.RunSync<string>(async () => {
                        //    //"qr_api_mapper.json" 
                        //    //var input= FileEndPointManager.InputWFFilePath;
                        //    result = await DoCmd(scriptFile);
                        //    return result;
                        //});

                        //MyBuilder.InvokeOnUiThreadIfRequired(.OpenFile(profile.SourcePath);
                        //?MyBuilder.InvokeOnUiThreadIfRequired(() => MyBuilder.OpenFile(profile.SourcePath));
                    }

                }
                //TIP: CreateResponse(HttpStatusCode.OK, result);  
                var configuration = new HttpConfiguration();
                HttpRequestMessage req = new HttpRequestMessage();
                req.SetConfiguration(configuration);
                return req.CreateResponse(HttpStatusCode.OK, "Done load script.");
            }

            ////STEP_ .RESULT #2 HandleCmd
            //else if (param.CommandName == EnumCmd.KatalonFailed.ToString())
            //{
            //    //STEP_.RESULT #4 raise error
            //   //MyBuilder.ExecuteAsync.RaiseError

            //    return new HttpResponseMessage(System.Net.HttpStatusCode.OK);


            //}



            //STEP_ .RESULT #2 HandleCmd
            else if (param.CommandName == EnumCmd.EndWFResult.ToString())
            {
                //STEP_.RESULT #4 Kill WF UI
                // Common.GlobalDesigner.IsWFScriptDone = true;
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK);


            }
            else
            {

                //step# 20 load WF
                //reset retry count
                // EventResponder.RetryCount = 0;
                RetryCount = 0;
                MyCmd = param;

                try
                {
                    if (param.CommandName == EnumCmd.RunWF.ToString())
                    {

                        //step# 45 call from event DO JUKEBOX NOW...
                        var cmd = (RunWFCmdParam)param.Payload; //JsonConvert.DeserializeObject<RunWFCmdParam>(param.Payload.ToString());

                        //var cmdParam = JsonConvert.DeserializeObject<Dictionary<string,string>>(cmd.Payload.ToString());
                        var cmdParameters = cmd.EventParameters;//JsonConvert.DeserializeObject<Dictionary<string,dynamic>>(cmd.Payload.ToString());

                        //STEP_.RECIEVER #306 save event file path
                        WFInputFilePath = cmd.EventFilePath;

                        //load wf from library
                        using (MyDbContext thisDb = new MyDbContext())
                        {

                            var Profiles = thisDb.WFProfiles.ToList();
                            var profile = Profiles.FirstOrDefault(p => p.Tag.ToLower() == cmd.EventName.ToLower());
                            if (profile != null)
                            {

                                //STEP.Receiver #500 
                                //    DevNoteIntegrationEventHandler.handle()
                                //    BotHttpClient.PostToDevNote(cmdCarrier);
                                //    then save to file
                                cmd.JSFullPath = profile.SourcePath;
                                FileEndPointManager.CreateInputWF(cmd);

                                //clean scriptengine windows
                                //foreach (var item in frmScriptBuilder.frmResults)
                                //{
                                //    try
                                //    {
                                //        item.engineInstance.tasktEngineUI.Invoke((Action)delegate () { item.Close(); });

                                //    }
                                //    catch (Exception)
                                //    {

                                //        //  throw;
                                //    }

                                //}



                                await AutoRun(cmd);


                                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(cmd.EventName + " event is executed.") };


                            }
                            else
                            {
                                //var r= new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);

                                //return new HttpResponseMessage(HttpStatusCode.NotFound)
                                //{ Content = new StringContent(SerializedString, System.Text.Encoding.UTF8, "application/json") };

                                return new HttpResponseMessage(HttpStatusCode.NotFound) { Content = new StringContent(cmd.EventName + " event is not registered in DevNote Library.") };

                            }


                        }

                        ////throw new NotImplementedException();
                        //var message = string.Format(">>{0} callback for: {1}", DateTime.Now.ToShortTimeString(), param.CommandName);
                        //LogApplication.Agent.LogInfo(message);
                        //return new HttpResponseMessage(System.Net.HttpStatusCode.OK);

                    }

                    return new HttpResponseMessage(HttpStatusCode.NotFound) { Content = new StringContent(param.CommandName + " command is not handled.") };


                }
                catch (Exception err)
                {

                    //STEP_.RESULT #901 if error occurs before Polly                  
                    await OutputManager.CreateErrorOutputWF(err.Message);

                    //await BotHttpClient.Log(err.Message, true);
                    Agent.LogError(err);
                    return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                }


            }






            //throw new NotImplementedException();
            var tempResult =  Task<HttpResponseMessage>.Factory.StartNew(() => {
                var model = new HttpResponseMessage(HttpStatusCode.OK)
                { Content = new StringContent("OpenBot agent run") };
                return model;
            });



            return result;

          

        }
    }
}
