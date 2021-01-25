//using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac;
using Steeroid.Business.Areas.Communication;
using Steeroid.Business.Machine;
using Steeroid.Infra2._0.Areas.Communication;
//using Steeroid.Infra2._0.Areas.Communication;
using Steeroid.Infra2._0.Areas.Machine;
using SteeroidXUnitTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Steeroid.Business.Areas.Communication.Tests
{
    public class BizBusServiceManagerAzureTest:BaseFixture
    {
        [Fact]
        public async void Step1_InitAzureInitAzureTest()
        {

            //prepare
            var mac = SteeroidXUnitTest.MyAutoFac.Container.Resolve<IMachineManager>();

            MyBusServiceManager azureFront =
              new MyBusServiceManager(mac); //(new MyMachineManager());

            var result = await azureFront.Step1_InitAzure();


            Assert.True(result.model);
        }
    }
}