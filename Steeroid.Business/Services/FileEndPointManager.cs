using AutoMapper;
//using Common.COMMANDS;
//using IntegrationEvents.Events.DevNote;
//using LogApplication.Common.Commands;
//Common.Config;
using Newtonsoft.Json;
//using OpenQA.Selenium;
using Steeroid.Models;
using Steeroid.Models.Common.Commands;
using Steeroid.Models.Enums;
using Steeroid.Models.Helpers;
using Steeroid.Models.Remoting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
//using taskt.DevNoteMod;

namespace Steeroid.Business.Services
{
    public static class FileEndPointManager
    {

        public static STEP_ Root { get; set; }


        static string _installFolder;
        public static string BaiSteeroidFolder
        {
            get
            {
                if (string.IsNullOrEmpty(_installFolder))
                {
                  
                    if(string.IsNullOrEmpty(_installFolder))
                    {
                        _installFolder = @"C:\BlastAsia\Steeroid";
                    }


                }
                return _installFolder;
            }
        }


        static string _myCommon;
        public static String MyCommon
        {
            get
            {
                //C:\BlastAsia\Common
                if (string.IsNullOrEmpty(_myCommon))
                    _myCommon = Path.Combine(BaiSteeroidFolder, "Common");

                return _myCommon;
            }
        }




        public static String MyTasktDirectory
        {
            get
            {
                //C:\BlastAsia\DevnoteTaskt
                if (string.IsNullOrEmpty(myTasktDirectory))
                    myTasktDirectory = BaiSteeroidFolder; //Path.Combine(InstallFolder, "DevnoteTaskt");

                return myTasktDirectory;
            }
        }



        public static string MyCodeceptTestTemplate
        {
            get
            {
                ConfigManager config = new ConfigManager();
                string template = config.GetValue("CodeceptTestTemplate");

                if (string.IsNullOrEmpty(template))
                {
                    //get default directory
                    //D:\_MY_PROJECTS\_DEVNOTE\_DevNote4\DevNote.Web.Recorder\Chrome\chrome-win\chrome.exe
                    var currentDir = Agent.GetCurrentDir();
                    currentDir = currentDir.Replace("file:\\", string.Empty);


                    var dir = string.Format("{0}\\CodeCeptJS\\Project2", currentDir);
                    template = System.IO.Path.Combine(dir, "template_test.txt");
                }
                return template;
            }

        }

        //static string _myCommonExeDirectory;
        //public static string MyCommonExeDirectory
        //{          

        //    get
        //    {

        //        if (string.IsNullOrEmpty(_myCommonExeDirectory))
        //        {

        //            var dir = string.Format("{0}\\Bat", FileEndPointManager.MyTasktDirectory);
        //            var dirExe = System.IO.Path.Combine(dir, "Exe");

        //            _myCommonExeDirectory = dirExe;

        //        }
        //        return _myCommonExeDirectory;

               
        //    }




        //}


        public static bool IsEventBusy
        {
            get
            {
                var files = Directory.GetFiles(MyWaitOneDirectory, "*.eve", SearchOption.TopDirectoryOnly);

                return files.Length > 0;
            }
        }

        public static String MyWaitOneDirectory
        {
            get
            {
                //STEP_.EVENT MyWaitOneDirectory
                if (string.IsNullOrEmpty(myWaitOneDirectory))
                {

                    var dir = string.Format("{0}\\Bat", FileEndPointManager.MyTasktDirectory);
                    var dirWaitOne = System.IO.Path.Combine(dir, "WaitOne");

                    myWaitOneDirectory = dirWaitOne;
                }
                return myWaitOneDirectory;
            }
        }
        static string _myOutcomeFolder;
        public static String MyOutcomeFolder
        {
            get
            {
                //STEP_.EVENT MyWaitOneDirectory
                if (string.IsNullOrEmpty(_myOutcomeFolder))
                {

                    var dir = string.Format("{0}\\Bat", FileEndPointManager.MyTasktDirectory);
                    var dirWaitOne = System.IO.Path.Combine(dir, "Outcome");

                    _myOutcomeFolder = dirWaitOne;
                }
                return _myOutcomeFolder;
            }
        }

        static string myEventDirectory;

        //TODO handle Event files
        public static String MyEventDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(myEventDirectory))
                {

                    var dir = string.Format("{0}\\Bat", FileEndPointManager.MyTasktDirectory);
                    var dirEvents = System.IO.Path.Combine(dir, "Events");

                    myEventDirectory = dirEvents;
                }
                return myEventDirectory;
            }
        }





        static string _myChromiumExe;
        
        public static string MyChromiumExe
        {
            get
            {
                if (string.IsNullOrEmpty(_myChromiumExe))
                {
                    //C:\BlastAsia\Common\Chrome\chrome-win
                    //Plugins\Tasktskie.StartProcessCommandKatalon\_EXE\Player2\Common\Chrome\chrome-win
                    var dir = @"C:\BlastAsia\Common\Chrome\chrome-win\";//Path.Combine(FileEndPointManager.MyCommon, @"Chrome\chrome-win");

                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    _myChromiumExe = System.IO.Path.Combine(dir, "Chrome.exe");
                   

                }
                return _myChromiumExe;
            }
        }




        public static void CreateInputWF(RunWFCmdParam cmd, bool isOverwrite = false)
        {

            var stringContent = JsonConvert.SerializeObject(cmd); //new StringContent(JsonConvert.SerializeObject(cmd), Encoding.UTF8, "application/json");
            var file = Path.Combine(FileEndPointManager.MyWaitOneDirectory, EnumFiles.WFInput);

            if (File.Exists(file) && isOverwrite == false)
            {
                File.WriteAllText(file, stringContent);
                return;
            }
            else
            {
             ClearOutputWF();
             File.WriteAllText(file, stringContent);
            }

        }

        public static  async Task<string> CreateEventInput(RunWFCmdParam cmd, bool isOverwrite = false)
        {

            var stringContent = JsonConvert.SerializeObject(cmd); //new StringContent(JsonConvert.SerializeObject(cmd), Encoding.UTF8, "application/json");

            if(!Directory.Exists(FileEndPointManager.MyEventDirectory))
            {
                Directory.CreateDirectory(FileEndPointManager.MyEventDirectory);
            }

            var file = Path.Combine(FileEndPointManager.MyEventDirectory
                , DateTime.Now.Ticks.ToString() + EnumFiles.WFInput);

            if (File.Exists(file) && isOverwrite == false)
            {
               // return file;
               //do nothing
            }
            else
            {
                // ClearOutputWF();
              await  Task.Factory.StartNew(() => File.WriteAllText(file, stringContent));
               // File.WriteAllText(file, stringContent);
            }

            return file;

        }


        //must be 1 reference only
        //only used by OutputManager!! 
        public static async Task<DevNoteIntegrationEvent> CreateOutputWF()
        {
            //STEP_.RESULT #99 CreateOutputWF
            var stringContent = File.ReadAllText(FileEndPointManager.InputWFFilePath);

            var cmd = ReadInputWFCmdJsonFile();
            var payload = cmd; //(RunWFCmdParam)cmd.Payload;

            var result = FileEndPointManager.ReadMyResultValueFile();



            //TIP# Configure AutoMapper
            var config = new MapperConfiguration(cfg => cfg.CreateMap<RunWFCmdParam, DevNoteIntegrationEvent>());
            var mapper = config.CreateMapper();
            // Perform mapping
            var @event = mapper.Map<RunWFCmdParam, DevNoteIntegrationEvent>(cmd);

            @event.GuidId = cmd.GuidId;
            @event.EventParameters = cmd.EventParameters;
            @event.EventName = cmd.EventName;
            @event.OuputResponse = result;
            @event.RetryCount = cmd.RetryCount;
            @event.ErrorCode = cmd.ErrorCode;
            @event.ReferenceId = cmd.ReferenceId;

            @event.MessageId = cmd.GuidId;
            @event.IsResponse = true;


            stringContent = JsonConvert.SerializeObject(@event);



            // var stringContent = JsonConvert.SerializeObject(cmd); //new StringContent(JsonConvert.SerializeObject(cmd), Encoding.UTF8, "application/json");
            var file = Path.Combine(FileEndPointManager.MyWaitOneDirectory, EnumFiles.WFOutput);



            //_HACK safe to delete 
            #region---TEST ONLY: Compiler will  automatically erase this in RELEASE mode and it will not run if Global.GlobalTestMode is not set to TestMode.Simulation
#if OVERRIDE || DEBUG

            //System.Diagnostics.Debug.WriteLine("HACK-TEST -");
            //await BotHttpClient.Log("FileEndPointManager.MyWaitOneDirectory:" + FileEndPointManager.MyWaitOneDirectory);
            //await BotHttpClient.Log("OuputResponse:" + result);


#endif
            #endregion //////////////END TEST



            //await BotHttpClient.Log("OuputValue:" + result);
            Agent.LogInfo("OuputValue:" + result);

            File.WriteAllText(file, stringContent);

            if (!string.IsNullOrEmpty(cmd.EventFilePath))
            {

                    try
                    {
                        if (File.Exists(cmd.EventFilePath))
                            File.Delete(cmd.EventFilePath);
                    }
                    catch (Exception err)
                    {

                    //await  BotHttpClient.Log(err.Message, true);
                    Agent.LogError(err);
                    }
            }

            //STEP_.RESULT #6 save to OUTCOME
            var fName = Path.GetFileName(cmd.EventFilePath);
            fName = fName.Replace(EnumFiles.WFInput, EnumFiles.WFOutput);

            file = Path.Combine(FileEndPointManager.MyOutcomeFolder, fName);
            File.WriteAllText(file, stringContent);

            //await BotHttpClient.Log("EventOutputStatus: " + Environment.NewLine + stringContent);
            Agent.LogInfo("EventOutputStatus: " + Environment.NewLine + stringContent);

            //var fileIn = Path.Combine(FileEndPointManager.MyWaitOneDirectory, EnumFiles.WFInput);
            //if (File.Exists(fileIn))
            //    File.Delete(fileIn);
            ClearInputWF();

            //delete Eventfile
            if (!string.IsNullOrEmpty(cmd.EventFilePath))
            {
                try
                {
                    if (File.Exists(cmd.EventFilePath))
                    {
                        File.Delete(cmd.EventFilePath);
                    }
                }
                catch (Exception err)
                {

                    //await BotHttpClient.Log(err.Message, true);
                    Agent.LogError(err);
                }
               
            }

            return @event;

        }


        //must be 1 reference only
        public static async Task<DevNoteIntegrationEvent> CreateErrorOutputWF(string errorMessage)
        {
            //STEP_.RESULT #99 CreateOutputWF
            var stringContent = File.ReadAllText(FileEndPointManager.InputWFFilePath);

            var cmd = ReadInputWFCmdJsonFile();
            var payload = cmd; //(RunWFCmdParam)cmd.Payload;

            var result = $"ERROR:{errorMessage}";  //FileEndPointManager.ReadMyGrabValueFile();

            //TIP# Configure AutoMapper
            var config = new MapperConfiguration(cfg => cfg.CreateMap<RunWFCmdParam, DevNoteIntegrationEvent>());
            var mapper = config.CreateMapper();
            // Perform mapping
            var @event = mapper.Map<RunWFCmdParam, DevNoteIntegrationEvent>(cmd);

            @event.GuidId = cmd.GuidId;
            @event.EventParameters = cmd.EventParameters;
            @event.EventName = cmd.EventName;
            @event.OuputResponse = result;
            @event.RetryCount = cmd.RetryCount;
            @event.ErrorCode = "101";//ErrorCodes cmd.ErrorCode;
            @event.ReferenceId = cmd.ReferenceId;

            @event.MessageId = cmd.GuidId;
            @event.IsResponse = true;


            stringContent = JsonConvert.SerializeObject(@event);



            // var stringContent = JsonConvert.SerializeObject(cmd); //new StringContent(JsonConvert.SerializeObject(cmd), Encoding.UTF8, "application/json");
            var file = Path.Combine(FileEndPointManager.MyWaitOneDirectory, EnumFiles.WFOutput);



            //await BotHttpClient.Log("OuputValue:" + result);
            Agent.LogInfo("OuputValue:" + result);
            File.WriteAllText(file, stringContent);

            if (!string.IsNullOrEmpty(cmd.EventFilePath))
            {

                try
                {
                    if (File.Exists(cmd.EventFilePath))
                        File.Delete(cmd.EventFilePath);
                }
                catch (Exception err)
                {

                    //await BotHttpClient.Log(err.Message, true);
                    Agent.LogError(err);
                }
            }

            //STEP_.RESULT #6 save to OUTCOME
            var fName = Path.GetFileName(cmd.EventFilePath);
            fName = fName.Replace(EnumFiles.WFInput, EnumFiles.WFOutput);

            file = Path.Combine(FileEndPointManager.MyOutcomeFolder, fName);
            File.WriteAllText(file, stringContent);

            //await BotHttpClient.Log("EventOutputStatus: " + Environment.NewLine + stringContent);
            Agent.LogInfo("EventOutputStatus: " + Environment.NewLine + stringContent);

            //var fileIn = Path.Combine(FileEndPointManager.MyWaitOneDirectory, EnumFiles.WFInput);
            //if (File.Exists(fileIn))
            //    File.Delete(fileIn);
            ClearInputWF();

            //delete Eventfile
            if (!string.IsNullOrEmpty(cmd.EventFilePath))
            {
                try
                {
                    if (File.Exists(cmd.EventFilePath))
                    {
                        File.Delete(cmd.EventFilePath);
                    }
                }
                catch (Exception err)
                {

                    // await BotHttpClient.Log(err.Message, true);
                    Agent.LogError(err);
                }

            }

            return @event;


        }




        public static void ClearInputWF()
        {
            // var stringContent = JsonConvert.SerializeObject(cmd); //new StringContent(JsonConvert.SerializeObject(cmd), Encoding.UTF8, "application/json");
            var file = Path.Combine(FileEndPointManager.MyWaitOneDirectory, EnumFiles.WFInput);
            // File.WriteAllText(file, stringContent);
            if (File.Exists(file))
                File.Delete(file);

        }


        public static void ClearOutputWF()
        {
            // var stringContent = JsonConvert.SerializeObject(cmd); //new StringContent(JsonConvert.SerializeObject(cmd), Encoding.UTF8, "application/json");
            var file = Path.Combine(FileEndPointManager.MyWaitOneDirectory, EnumFiles.WFOutput);
            // File.WriteAllText(file, stringContent);
            if (File.Exists(file))
                File.Delete(file);

        }



        public static CmdParam ReadCmdJsonFile(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var cmd = JsonConvert.DeserializeObject<CmdParam>(json);
            return cmd;

        }

        public static string InputWFFilePath
        {
            get
            {
                 return Path.Combine(FileEndPointManager.MyWaitOneDirectory, EnumFiles.WFInput);

            }

        }
        public static string OutputWFFilePath
        {
            get
            {
                return Path.Combine(FileEndPointManager.MyWaitOneDirectory, EnumFiles.WFOutput);

            }

        }
        public static RunWFCmdParam ReadInputWFCmdJsonFile()
        {
            var cmd = new RunWFCmdParam
            {
                EventParameters = new Dictionary<string, string>()
            };

            if (File.Exists(InputWFFilePath))
            {
                var json = File.ReadAllText(InputWFFilePath);
                cmd = JsonConvert.DeserializeObject<RunWFCmdParam>(json);
               
            }
           
             return cmd;
            

        }

        public static DevNoteIntegrationEvent ReadOuputWFCmdJsonFile()
        {
            var @event = new DevNoteIntegrationEvent();
           
            if (File.Exists(OutputWFFilePath))
            {
                var json = File.ReadAllText(OutputWFFilePath);
                @event = JsonConvert.DeserializeObject<DevNoteIntegrationEvent>(json);

            }

            return @event;


        }


        public static bool IsWFBusy
        {
            get
            {
                var files = Directory.GetFiles(MyWaitOneDirectory, EnumFiles.WFOutput, SearchOption.TopDirectoryOnly);            
                bool isOuput = files.Length > 0;

                var filesIn = Directory.GetFiles(MyWaitOneDirectory, EnumFiles.WFInput, SearchOption.TopDirectoryOnly);

                bool isInput = filesIn.Length > 0;

                if (isInput && isOuput == false)
                {
                    return true;
                }
                else
                    return false;


            }
        }


        public static bool IsTasktClear
        {
            get
            {
                var files = Directory.GetFiles(MyWaitOneDirectory,"*.*", SearchOption.TopDirectoryOnly);
                bool isOuput = files.Length == 0;
                return isOuput;

            }
        }
        public static void ClearTasktOutput()
        {
            var files = Directory.GetFiles(MyWaitOneDirectory, "*.*", SearchOption.TopDirectoryOnly).ToList();

            foreach (var item in files)
            {
                File.Delete(item);
            }          

        }

     


        static string _defaultPlayJsFile;
        public static string DefaultPlayJsFile
        {
            get
            {
                if (string.IsNullOrEmpty(_defaultPlayJsFile))
                {
                    ConfigManager config = new ConfigManager();
                    var file = config.GetValue("DefaultPlayJsFile");

                    //STEP_.PLAYER CHROME DOWNLOAD FOLDER
                    //SERVER: use the main folder of devnote.main
                    if (string.IsNullOrEmpty(file))
                    {
                        //D:\_MY_PROJECTS\_DEVNOTE\_DevNote4\DevNote.Main\bin\Debug2\_EXE\Player\CodeCeptJS\Project2
                        //var dir = string.Format("{0}\\_EXE\\Player\\CodeCeptJS\\Project2", FileEndPointManager.MyMainDirectory);
                        _defaultPlayJsFile = System.IO.Path.Combine(Project2Folder, "latest_test.js");
                    }
                    //CLIENT
                    else //if supplied used for stand alone player
                        _defaultPlayJsFile = file;
                }
                return _defaultPlayJsFile;
            }
        }


        static string _defaultLatestXMLFile;
        [Obsolete]
        public static string DefaultLatestXMLFile
        {
            get
            {
                if (string.IsNullOrEmpty(_defaultLatestXMLFile))
                {

                    _defaultLatestXMLFile = System.IO.Path.Combine(Project2Folder, "latest.xml");

                }
                return _defaultLatestXMLFile;
            }
        }


        static string _defaultLatestTasktXML;
        public static string DefaultLatestTasktXML
        {
            get
            {
                if (string.IsNullOrEmpty(_defaultLatestTasktXML))
                {

                    _defaultLatestTasktXML = Path.Combine(FileEndPointManager.MyCommon, "latestTaskt.xml");

                }
                return _defaultLatestTasktXML;
            }
        }

        static string _katalonLog;
        public static string KatalonLog
        {
            get
            {
                if (string.IsNullOrEmpty(_katalonLog))
                {

                    _katalonLog = Path.Combine(FileEndPointManager.MyCommon, "katalonLog.txt");

                }
                return _katalonLog;
            }
        }



        static string _defaultLatestHtmlFile;
        public static string DefaultLatestHtmlFile
        {
            get
            {
                if (string.IsNullOrEmpty(_defaultLatestHtmlFile))
                {
                    var dir = Path.Combine(FileEndPointManager.MyCommon, "Temp");

                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    _defaultLatestHtmlFile =Path.Combine(FileEndPointManager.MyCommon, @"Temp\latest.html");

                }
                return _defaultLatestHtmlFile;
            }
        }


        static string _defaultTempHtmlFile;
        public static string DefaultTempHtmlFile
        {
            get
            {
                if (string.IsNullOrEmpty(_defaultTempHtmlFile))
                {
                    var dir = Path.Combine(FileEndPointManager.MyCommon, "Temp");
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    // _defaultLatestHtmlFile = Path.Combine(Project2Folder, "latest.html");
                    _defaultTempHtmlFile = Path.Combine(FileEndPointManager.MyCommon, @"Temp\temp.html");
                }
                return _defaultTempHtmlFile;
            }
        }

        static string _defaultTempTasktFile;
        public static string DefaultTempTasktFile
        {
            get
            {
                if (string.IsNullOrEmpty(_defaultTempTasktFile))
                {
                    var dir = Path.Combine(FileEndPointManager.MyCommon, "Temp");
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    // _defaultLatestHtmlFile = Path.Combine(Project2Folder, "latest.html");
                    _defaultTempTasktFile = Path.Combine(FileEndPointManager.MyCommon, @"Temp\temp.xml");
                }
                return _defaultTempTasktFile;
            }
        }



        static string _defaultLoader;
        public static string DefaultLoader
        {
            get
            {
                if (string.IsNullOrEmpty(_defaultLoader))
                {
                    var dir = Path.Combine(FileEndPointManager.MyTasktDirectory, "Assets");
                    //if (!Directory.Exists(dir))
                    //    Directory.CreateDirectory(dir);
                    _defaultLoader = Path.Combine(dir, @"Loading.html");
                 

                }
                return _defaultLoader;
            }
        }


        static string _defaultLatestResultFile;
        public static string DefaultLatestResultFile
        {
            get
            {
                if (string.IsNullOrEmpty(_defaultLatestResultFile))
                {
                    var dir = Path.Combine(FileEndPointManager.MyCommon, "Temp");

                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);


                    // _defaultLatestHtmlFile = Path.Combine(Project2Folder, "latest.html");
                    return Path.Combine(FileEndPointManager.MyCommon, @"Temp\result.txt");

                }
                return _defaultLatestResultFile;
            }
        }

        static string _defaultCSV;
        public static string DefaultCSV
        {
            get
            {
                if (string.IsNullOrEmpty(_defaultCSV))
                {
                    var dir = Path.Combine(FileEndPointManager.MyCommon, "Temp");

                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);


                    // _defaultLatestHtmlFile = Path.Combine(Project2Folder, "latest.html");
                    return Path.Combine(FileEndPointManager.MyCommon, @"Temp\scrape.xlsx");

                }
                return _defaultCSV;
            }
        }

        static string _defaultExcelScrapeXLSM;
        public static string DefaultExcelScrapeXLSM
        {
            get
            {
                if (string.IsNullOrEmpty(_defaultExcelScrapeXLSM))
                {
                    var dir = Path.Combine(MyTasktDirectory, "Templates");

                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);


                    // _defaultLatestHtmlFile = Path.Combine(Project2Folder, "latest.html");
                    return Path.Combine(dir, "scrape.xlsm");

                }
                return _defaultExcelScrapeXLSM;
            }
        }



        static string  _defaultScriptFolder;
        public static string DefaultScriptFolder
        {
            get
            {
                if (string.IsNullOrEmpty(_defaultScriptFolder))
                {
                    //ConfigManager config = new ConfigManager();
                    //var file = config.GetValue("DefaultXMLFile");

                   var file = Path.Combine(MyTasktDirectory, "Scripts");
                    _defaultScriptFolder = file;
                }
                return _defaultScriptFolder;
            }
           
        }

        static string _defaultSnippetFolder;
        public static string DefaulSnippetFolder
        {
            get
            {
                if (string.IsNullOrEmpty(_defaultSnippetFolder))
                {
                    //ConfigManager config = new ConfigManager();
                    //var file = config.GetValue("DefaultXMLFile");

                    var file = Path.Combine(MyTasktDirectory, "Snippets");
                    _defaultSnippetFolder = file;
                }
                return _defaultSnippetFolder;
            }

        }
        static string _defaultXAMLFolder;
        public static string DefaultXAMLFolder
        {
            get
            {
                if (string.IsNullOrEmpty(_defaultXAMLFolder))
                {
                    //ConfigManager config = new ConfigManager();
                    //var file = config.GetValue("DefaultXMLFile");

                    var file = Path.Combine(MyTasktDirectory, "XAML");
                    _defaultXAMLFolder = file;
                }
                return _defaultXAMLFolder;
            }

        }



        #region ROOT Dynmic

        //    <add key="Project2EndPointFolder" 
        //value="D:\_MY_PROJECTS\_DEVNOTE\_DevNote4\DevNote.Web.Recorder\bin\Debug\CodeCeptJS\Project2\output\endpoint" />

        static string _project2EndPointFolder;

        [Obsolete]
        public static String Project2EndPointFolder
        {
            get
            {

                return DefaultLatestResultFile;
                //_project2EndPointFolder = Path.Combine(Project2Folder, "output");
                //var dirOutput = System.IO.Path.Combine(_project2EndPointFolder, "endpoint");

                //return dirOutput; //_project2EndPointFolder;
            }
        }

        static string _project2Folder;

        [Obsolete]
        public static string Project2Folder
        {
           
                get
                      {


                ConfigManager config = new ConfigManager();
                _project2Folder = config.GetValue("Project2Folder");


                if (Root == STEP_.PLAYER)
                    {
                        if (string.IsNullOrEmpty(_project2Folder))
                        {
                            var dir = Agent.GetCurrentDir();
                            myTasktDirectory = dir.Replace("file:\\", string.Empty);
                            var exeFolder1 = string.Format("{0}\\CodeCeptJS\\Project2", MyTasktDirectory);
                            _project2Folder = exeFolder1;
                        }

                    }
                    else
                    {
                        //STEP_.EVENT Project2EndPointFolder
                        if (string.IsNullOrEmpty(_project2Folder))
                        {

                            var dir = string.Format("{0}\\_EXE\\Player\\CodeCeptJS\\Project2", MyTasktDirectory);
                           _project2Folder = dir;
                        }
                    }

                    return _project2Folder;
                }
            }

        [Obsolete]
        public static string MyPlayerExe
        {
            get
            {
                var exe = string.Format("{0}\\_EXE\\Player\\DevNote.Web.Recorder.exe", MyTasktDirectory);

                return exe;

            }
        }



        public static string MyPlayerExe2
        {
            get
            {
                var exe = string.Format("{0}\\_EXE\\Player2\\DevNotePlay2.exe", MyTasktDirectory);

                return exe;

            }
        }

        public static string DefaultTasktTemplate
        {
            get
            {
                var exe = string.Format("{0}\\Bat\\DefaultTasktTemplate.xml", MyTasktDirectory);
                return exe;

            }
        }


        //static string _chromeDownLoad;
        //public static String ChromeDownLoad
        //{
        //    get
        //    {
        //        ConfigManager config = new ConfigManager();
        //        _chromeDownLoad= config.GetValue("ChromeDownloadFolder");

        //        if (string.IsNullOrWhiteSpace(_chromeDownLoad))
        //            return Project2Folder;


        //        return _chromeDownLoad; //_project2EndPointFolder;
        //    }
        //}



        [Obsolete]
        public static string Latest_testJS()
        {
            var @result = string.Empty;
            var endPointFolder =   Path.Combine(MyTasktDirectory, "XAML");


            var file = Path.Combine(endPointFolder, EnumFiles.MyGrabValue);

            if (File.Exists(file))
            {
                result = File.ReadAllText(file);

            }

            return result;



        }

        public static string ReadMyResultValueFile()
        {
            var @result = string.Empty;
            var file = DefaultLatestResultFile;//Project2EndPointFolder;
            //var file = Path.Combine(endPointFolder, EnumFiles.MyGrabValue);

            if (File.Exists(file))
            {
                result = File.ReadAllText(file);

            }

            return result;

        }


        #endregion



        #region OUTPU

        public static void CreateLatestResultFile(string latestContent)
        {
            //string latestXML = FileEndPointManager.DefaultLatestXMLFile;
            string latest = DefaultLatestResultFile;//.DefaultLatestHtmlFile;        


            if (File.Exists(latest))
                File.Delete(latest);

            //File.WriteAllText(latestXML, xmlContent);
            File.WriteAllText(latest, latestContent);
        }

        #endregion



        private static string myTasktDirectory;
        private static string myWaitOneDirectory;
        // private static string myOutputWFDirectory;

        #region SYNC CustomConfig

        public static void SyncCustomConfig()
        {
            //update config
            //STEP_.INIT UPdate ALL COnfig Sync Custom.Config

            var dir = Agent.GetCurrentDir();
            myTasktDirectory = dir.Replace("file:\\", string.Empty);

            //set root folder for all
            ConfigManager config = new ConfigManager();
            config.SetValue(MyConfig.MyMainFolder.ToString(), myTasktDirectory);



            //Project2Folder
            config.SetValue(MyConfig.Project2Folder.ToString(), Project2Folder);

          

            //var exeFolder1 = string.Format("{0}\\_EXE\\Receiver", myMainDirectory);
            //var exePath = Path.Combine(exeFolder1, "EFCoreTransactionsReceiver.exe");
            //config.SetValue(MyConfig.AzureServiceBusReceiver.ToString(),exePath);

            //var exeFolder2 = string.Format("{0}\\_EXE\\Sender", myMainDirectory);
            // exePath = Path.Combine(exeFolder2, "EFCoreTransactionsSender.exe");
            // config = new ConfigManager();
            // config.SetValue(MyConfig.AzureServiceBusSender.ToString(), exePath);


            //var exeFolder3 = string.Format("{0}\\_EXE\\Designer", myMainDirectory);
            //exePath = Path.Combine(exeFolder3, "BaiCrawler.exe");
            //config.SetValue(MyConfig.DevNoteDesingnerExe.ToString(), exePath);


            var exeFolder4 = string.Format("{0}\\_EXE\\Player2", myTasktDirectory);
           // var exePath = Path.Combine(exeFolder4, "DevNote.Web.Recorder.exe");
            var exePath = Path.Combine(exeFolder4, "DevNotePlay2.exe");
        
            config.SetValue(MyConfig.DevNotePlayerExe.ToString(), exePath);



            //var exeFolder5 = Project2EndPointFolder;
            //config.SetValue(MyConfig.Project2EndPointFolder.ToString(), exeFolder5);
           // config.SetValue("CommonExeFolder", MyCommonExeDirectory);




            //exeFolder = string.Format("{0}\\_EXE\\Player", myMainDirectory);
            //exePath = Path.Combine(exeFolder, "chrome.exe");
            //config = new ConfigManager();
            //config.SetValue(MyConfig.ChromeExe.ToString(), exePath);


            //copy to all Concern Exe
            var configFolder = myTasktDirectory;
            var source = Path.Combine(configFolder, "Custom.config");

            //var dest1 = Path.Combine(exeFolder1  , "Custom.config");
            //File.Copy(source, dest1, true);

            //var dest2 = Path.Combine(exeFolder2, "Custom.config");
            //File.Copy(source, dest2, true);

            //var dest3 = Path.Combine(exeFolder3, "Custom.config");
            //File.Copy(source, dest3, true);
            config.Save();
            var dest4 = Path.Combine(exeFolder4, "Custom.config");
            File.Copy(source, dest4, true);

        }

        public static void SyncDatabaseConfig()
        {
            //D:\_MY_PROJECTS\_DEVNOTE\_DevNote4\DevNote.Main\bin\Debug2
            var dir = Agent.GetCurrentDir();
            myTasktDirectory = dir.Replace("file:\\", string.Empty);

            //FileEndPointManager.MyCommon

            var templatePath = Path.Combine(myTasktDirectory, "Common.config.txt");
            var templateConfig = File.ReadAllText(templatePath);

            //make it permanent
            var actualPath =Path.Combine(MyCommon, "MyDBContext.sdf");// @"C:\Blastasia\Common\MyDbContext.sdf"; 



            var finalTxt = templateConfig.Replace("##DbFullPath##", actualPath);

            var myCommonConfig = Path.Combine(myTasktDirectory, "common.config");
            File.WriteAllText(myCommonConfig, finalTxt.Trim());

            var source = myCommonConfig;

            //var exeFolder1 = string.Format("{0}\\_EXE\\Receiver", myMainDirectory);
            //var commonPath = Path.Combine(exeFolder1, "common.config");
            //File.Copy(source, commonPath, true);

            //var exeFolder2 = string.Format("{0}\\_EXE\\Sender", myMainDirectory);
            //commonPath = Path.Combine(exeFolder2, "common.config");
            //File.Copy(source, commonPath, true);


            //var exeFolder3 = string.Format("{0}\\_EXE\\Designer", myMainDirectory);
            //commonPath = Path.Combine(exeFolder3, "common.config");
            //File.Copy(source, commonPath, true);


            var exeFolder4 = string.Format("{0}\\_EXE\\Player2", myTasktDirectory);
            var commonPath = Path.Combine(exeFolder4, "common.config");
            File.Copy(source, commonPath, true);

        }

        public static void SyncLogConfig()
        {

            //initializeData="Logs\{ApplicationName}-{DateTime:yyyy-MM-dd}.log"
            var keyWord = @"{ApplicationName}-{DateTime:yyyy-MM-dd}.log";
            var dir = Agent.GetCurrentDir();
            myTasktDirectory = dir.Replace("file:\\", string.Empty);
            var logFolder = Path.Combine(myTasktDirectory, "Logs");
            var replaceWord = Path.Combine(logFolder, keyWord);


            var configTemplate = Path.Combine(myTasktDirectory, "CommonLog.config.txt");
            var templateConfig = File.ReadAllText(configTemplate);

            var finalTxt = templateConfig.Replace(@"##LogPath##", replaceWord);

            var myCommonConfig = Path.Combine(myTasktDirectory, "CommonLog.config");
            File.WriteAllText(myCommonConfig, finalTxt.Trim());

            var source = myCommonConfig;

            //var exeFolder1 = string.Format("{0}\\_EXE\\Receiver", myMainDirectory);
            //var commonPath = Path.Combine(exeFolder1, "CommonLog.config");
            //File.Copy(source, commonPath, true);

            //var exeFolder2 = string.Format("{0}\\_EXE\\Sender", myMainDirectory);
            //commonPath = Path.Combine(exeFolder2, "CommonLog.config");
            //File.Copy(source, commonPath, true);


            //var exeFolder3 = string.Format("{0}\\_EXE\\Designer", myMainDirectory);
            //commonPath = Path.Combine(exeFolder3, "CommonLog.config");
            //File.Copy(source, commonPath, true);


            var exeFolder4 = string.Format("{0}\\_EXE\\Player2", myTasktDirectory);
            var commonPath = Path.Combine(exeFolder4, "CommonLog.config");
            File.Copy(source, commonPath, true);





        }



        #endregion



    }
}
