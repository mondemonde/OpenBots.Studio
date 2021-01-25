using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Newtonsoft.Json;
using Steeroid.Business.Areas.Bot;
using Steeroid.Business.Areas.Input.Commands;
using Steeroid.Business.Recorodings.Query;
using Steeroid.Business.Services;
using Steeroid.Models;
using Steeroid.Models.Enums;
using Steeroid.Models.Helpers;
using Steeroid.Models.Remoting;

namespace Steeroid.Business.Areas.Input
{
    public  class BizInputManager
    {

        IMediator _mediator;


        public BizInputManager()
        {
            //_botAgent = bot;
            _mediator = BizService.Resolve<IMediator>();
            _isPaused = true;

        }

        public async Task StartFileMonitor()
        {
            if (IsPaused == false)//running
                return;

            _isPaused = false;
            await MonitorInput();

        }

        public void StopFileMonitor()
        {
            _isPaused = true;         

        }


        async Task MonitorInput()
        {
            while (IsPaused==false)
            {
                //DoSomething(); // takes 300ms on average
                 await CheckFiles();

                await Task.Delay(3000);
            }
        }

     public async Task CheckFiles()
        {
            #region AZURE EVENT--------

            var eventFiles = Directory.GetFiles(FileEndPointManager.MyEventDirectory
                , "*" + EnumFiles.WFInput.ToString(), SearchOption.TopDirectoryOnly)
                .ToList().OrderBy(x => x);

            var eventFile = eventFiles.FirstOrDefault();


            if (!string.IsNullOrEmpty(eventFile) && FileEndPointManager.IsWFBusy == false)
            {
                //TIP: wait process
                //TaskAwaiter cond = new TaskAwaiter("ShowGoogleChrome1");
                //var rawResponse = await cond.WaitUntilChromeIsRunnung();                 

                if (FileEndPointManager.IsTasktClear == true)

                {
                    try
                    {
                        var content = File.ReadAllText(eventFile);
                        //STEP.Main #9500 Move to file consumption

                        var runCmd = JsonConvert.DeserializeObject<RunWFCmdParam>(content);

                        //var intro = cmd.Message;
                        string msg = "Current Event: " + runCmd.EventName;
                        int fileCnt = eventFiles.Count() - 1;
                        msg += Environment.NewLine + "Pending Events: " + fileCnt.ToString();
                        //BotHttpClient.UpdateMainUI(msg);
                        Console.WriteLine(msg);
                        //package to cmd
                        RunWFCmdParam cmdCarrier = new RunWFCmdParam();

                        if (runCmd.EventParameters == null)
                            runCmd.EventParameters = new Dictionary<string, string>();

                        //STEP_.RECIEVER #305 runCmd.EventFilePath = eventFile;
                        runCmd.EventFilePath = eventFile;
                        cmdCarrier.Payload = runCmd;                   

                        //lock the process now
                        FileEndPointManager.CreateInputWF(runCmd);

                        AutoRunFromFileCmd command = new AutoRunFromFileCmd
                        {
                            Params = cmdCarrier
                        };

                        var response = await _mediator.Send(command);


                        //STEP_.RECIEVER this done only by this timer...
                        //call the designer to load the WF
                        // _ = await BotHttpClient.PostToDevNote(cmdCarrier);

                        //Ibot bot = new TasktAgent(this);
                        //var cmdResult = await _botAgent.HandleCmd(cmdCarrier);//AutoRun(); //await MyDesigner.HandleCmd(value);

                    }
                    catch (Exception err)
                    {
                        //clear erring file
                        if(File.Exists(eventFile))
                          File.Delete(eventFile);

                        Agent.LogWarn("File is deleted, maybe an invalid file: " + eventFile);

                        Agent.LogError(err);
                        //  throw;
                    }

                }

            }

            #endregion //end azure------

        }


        public async Task<string> CreateInputFile(DevNoteCmdEvent @event)
        {


            var result = string.Empty;
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

                    var response = await BizService.Mediator.Send(searchCmd);


                    if (!response.HasError)
                    {
                        var record = response.Model;

                        //CodeceptCmdParam
                        //STEP_.EVENT CreateEventInput FileEnpoint here  
                        cmd.JSFullPath = record.SourcePath;


                        result = await FileEndPointManager.CreateEventInput(cmd);


                    }


                    else
                    {
                        Agent.LogError("No record found for event " + cmd.EventName);
                        cmd.JSFullPath = string.Empty;
                        cmd.ErrorCode = ErrorCodes.FileNotFound.ToString(); //"404";//ErrorCodes.FileNotFound;
                        result = await FileEndPointManager.CreateEventInput(cmd);

                    }


                }


            }
            catch (Exception err)
            {

                Agent.LogError(err.Message);
            }

            return result;

        }

        bool _isPaused;
        public bool IsPaused { get=> _isPaused; }

        //public string 

    }
}
