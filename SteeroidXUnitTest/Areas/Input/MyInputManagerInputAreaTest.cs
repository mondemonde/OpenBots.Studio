using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Steeroid.Business.Areas.Input;
using Steeroid.Business.Services;
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

namespace Steeroid.Infra2._0.Areas.Input.Tests
{
    [Collection("Non-Parallel Collection")]
    public class MyInputManagerInputAreaTest: BaseFixture
    {
        [Fact]
        public async void Step1_TriggerAutomation()
        {
            Agent.Retest();

            //add new wfinput file
            FileTestHelper.ClearInputFiles();
            var dir = Agent.GetCurrentDir();
            var file = Path.Combine(dir, @"Resources\TestWFInput.json");
            var dest = Path.Combine(FileEndPointManager.MyEventDirectory, "TestWFInput.json");
            File.Copy(file, dest);

            //clear waitone directory
            FileEndPointManager.ClearInputWF();

            BizInputManager fileManager = new BizInputManager();

            await fileManager.CheckFiles();

            Assert.True(Agent.Test.Exists(x => x.Name == "AutoRunCmd"), "AutoRunCmd is not triggered.");

            //clear waitone directory
            FileTestHelper.ClearInputFiles();
            FileEndPointManager.ClearInputWF();


        }

     
    }
}