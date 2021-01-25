using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Steeroid.Models.Common.Commands;
//using System.Windows.Forms;
using Steeroid.Models.Interfaces.Plugins;
using Steeroid.Models.Interfaces.Scripting;
using Steeroid.Models.Remoting;

namespace Steeroid.Business.Areas.Bot
{
    //public interface IfrmScriptBuilder
    public interface IOPenBot

    {
        CmdParam MyCmd { get; set; }
        int RetryCount { get; set; }
        string WFInputFilePath { get; set; }

        Task<HttpResponseMessage> AutoRun(RunWFCmdParam cmd);
        Task<HttpResponseMessage> AutoRunCodeCept(RunWFCmdParam cmd);
        Task<HttpResponseMessage> AutoRunTest();
        Task<HttpResponseMessage> HandleCmd(CmdParam param);





        //    string ActualFile { get; }
        //    int DebugLine { get; set; }
        //    IScriptDirector MyScriptDirector { get; }
        //    string notificationText { get; set; }
        //    string ScriptFilePath { get; set; }
        //    Task<bool> AutoRunPolly(RunWFCmdParam cmd);
        //    void OpenFile(string filePath);



        //frmScriptBuilder parentBuilder { get; set; }
        //List<ListViewItem> RowsSelectedForCopy { get; set; }
        //void AddCommandToListView(IScriptCommand selectedCommand);


        //event EventHandler RecordWeb;
        //void ApplyEditorFormat();
        //void AutoRun(RunWFCmdParam cmd);
        //void AutoRun(string file);
        //void InvokeOnUiThreadIfRequired(Action action);
        //void LoadSteerMenu();
        //void LoadUI();
        //void LoadView();
        //void Notify(string notificationText, EnumAlertLevel alert = EnumAlertLevel.Green);
        //void PopulateExecutionCommands(List<ScriptAction> commandDetails);
    }

}
