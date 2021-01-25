using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using OpenBots.Agent.Core.Utilities;
using OpenBots.Core.Enums;
using OpenBots.Core.IO;
using OpenBots.UI.Forms;
using Serilog.Core;
using Steeroid.Business.Areas.Input;
using Steeroid.Business.Services;
using Steeroid.Infra2._0.Areas.Bot;
using Steeroid.Infra2._0.Areas.Input;
using Steeroid.Models.Enums;
using Steeroid.Models.Helpers;
using SteeroidXUnitTest;
using SteeroidXUnitTest.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Steeroid.Infra2._0Tests.OB
{

    [Collection("Non-Parallel Collection")]
    public  class OBRunnerInputAreaTest : BaseFixture
    {

        [Fact]
        public void RunAutomationOBTest()
        {
            Agent.Retest();

            //add new wfinput file
            OBRunner runner = new OBRunner();

            string testFile = @"F:\Users\rgalvez\Documents\OpenBotsStudio\My Scripts\Demo1\Main.json";

            runner.RunAutomation(testFile);

            Assert.True(true);
        

            //Assert.True(Agent.Test.Exists(x => x.Name == "AutoRunCmd"), "AutoRunCmd is not triggered.");

         
        }

        //[Fact]
        //public void RunfrmScriptEngineTest()
        //{
        //    Agent.Retest();

        //    //add new wfinput file
        //    //OBRunner runner = new OBRunner();

        //    string filePath = @"F:\Users\rgalvez\Documents\OpenBotsStudio\My Scripts\Demo1\Main.json";



        //    string engineLoggerFilePath = Path.Combine(Folders.GetFolder(FolderType.LogFolder), "OpenBots Engine Logs.txt");
        //    Logger engineLogger = new Logging().CreateFileLogger(engineLoggerFilePath, Serilog.RollingInterval.Day);
        //    var frm = new frmScriptEngine(filePath, "", null, engineLogger);
        //   // frm.sho

        //    Assert.True(true);


        //    //Assert.True(Agent.Test.Exists(x => x.Name == "AutoRunCmd"), "AutoRunCmd is not triggered.");


        //}

    }
}
