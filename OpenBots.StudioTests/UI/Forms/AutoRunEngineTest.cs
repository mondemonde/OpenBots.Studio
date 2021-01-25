using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenBots.Agent.Core.Utilities;
using OpenBots.Core.Enums;
using OpenBots.Core.IO;
using OpenBots.UI.Forms;
using Serilog.Core;
using Steeroid.Business.Areas.Output;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBots.UI.Forms.Tests
{
    [TestClass()]
    public class AutoRunEngineTest
    {
        [TestMethod()]
        public void RunScriptFileTest()
        {
            Steeroid.Models.Helpers.Agent.Retest();

            string filePath = @"F:\Users\rgalvez\Documents\OpenBotsStudio\My Scripts\Demo1\Main.json";



            string engineLoggerFilePath = Path.Combine(Folders.GetFolder(FolderType.LogFolder), "OpenBots Engine Logs.txt");
            Logger engineLogger = new Logging().CreateFileLogger(engineLoggerFilePath, Serilog.RollingInterval.Day);
            var frm = new frmScriptEngine(filePath, "", null, engineLogger,null,null,null,false,true);

            OutputManager.IsTasktDone = false;
            Steeroid.Models.Helpers.Agent.LogTest("RunScriptFileTest");
            frm.CloseWhenDone = true;


            frm.ShowDialog();

            Assert.IsTrue(OutputManager.IsTasktDone == true);
        }
    }
}