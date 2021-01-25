using System;
using System.Collections.Generic;
using System.Text;

namespace Steeroid.Models.Enums
{
    public enum LicenseMode
    {
        None = 0,
        MachineRpaServer = 11,
        MachineRpaBot = 12,
        Application = 101,
        NopCommerce = 102,
        QuickReach = 103,
        TokenOnly = 201
    }

    public enum MSG
    {
        DomainName,
        GlobalMachineId,
        Mode,
        ReferenceId,


        fx_setting_SendUrl,
        fx_setting_Code,

        fx_setting_SendUrl_Steer,
        fx_setting_Code_Steer,
        fx_QR_AppId = 9,

        SvcBusConnString,
        Topic,
    }

    public enum EnumSynch
    {
        NONE,
        DEVHUB_UPDATE,
        DEVHUB_DELETE,
        DEVHUB_INSERT,
        DEVFRONT_RUN,
        DEVHUB_SAVE_OUTPUT

    }


    public enum STEP_
    {
        INIT = 0,

        AZURE_BUS=100,

        MAIN = 900,
        BLAZOR = 1700,
        PLAYER = 800,
        AutoRun=850,

        RECIEVER = 500,
        SENDER = 600,

      
        LICENSE = 1100,
        RECORDS = 1200,
        //CodeCept = 80,
        EVENT = 300,
        RESULT=5000,

        AUTOFAC

    }

    public enum EnumPlayStatus
    {

        TimeOut = -10,//or TimedOut

        Faulted = -1,

        Success = 0,

        Idle = 5,

        Playing = 10,

        Retrying = 20
    }

    public enum EnumCmd
    {
        //actions
        GoTo,
        TypeKey,
        Click,
        Scroll,
        Submit,
        SaveToFile,
        Email,
        Puppet,
        Debug,
        Find,
        ConsoleTest,
        Codecept,
        Initialize,
        Screenshot,

        //Main UI
        UpdateMainView,

        //status
        ChromeStarted,
        ChromeBusy,
        ChromeClosing,

        //project
        StartBotProject,
        BotProjectCompleted,
        BotProjectRunning,
        BotStep,

        //WF
        RunWF,
        EndWFResult,
        TasktVars,
        TasktVarsUpdate,
        TasktVarsOpen,
        //KatalonFailed,

        //Database
        DBGetEvents,
        DBGetParameters


    }

    public static class EnumFiles
    {
        public static string Receiver = "RunAzureReceiver.bat";
        public static string Sender = "RunAzureSender.bat";
        public static string Designer = "RunDesigner.bat";
        public static string Player = "RunPlayer.bat";
        public static string RunDevNoteMain = "RunDevNoteMain.txt";

        public static string WFOutput = "WFOutput.json";

        public static string WFInput = "WFInput.json";

        public static string EventResult = "EventResult.json";


        public static string MyGrabValue = "MyGrabValue.txt";
        public static string MyResult = "result.txt";

        public static string MinimizeAll = "MinimizeAll.bat";

        public static string UI_Prefix = "UI_";


    }

    public enum MyConfig
    {
        MyMainFolder,
        DevNotePlayerExe,
        ChromeExe,
        ScreenshotOnFail,
        Project2Folder,
        Project2EndPointFolder


    }


    // DefaultApiPort see ..
    //F:\repos\OpenBots.Studio\Steeroid.Business\Areas\Communication\WebApi\DefaultApiPort.cs

}
