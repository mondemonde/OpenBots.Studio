//using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac;
using Steeroid.Business.Areas.Communication;
using Steeroid.Infra2._0.Areas.Communication;
//using Steeroid.Infra2._0.Areas.Communication;
using Steeroid.Infra2._0.Areas.Machine;
using SteeroidUnitTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Steeroid.Business.Areas.Communication.Tests
{
    public class BizBusServiceManagerAzureTest
    {
        [Fact]
        public async void Step1_InitAzureInitAzureTest()
        {


            //prepare
            var mac= MyAutoFac.Container.Resolve<MyMachineManager>();
            MyBusServiceManager azureFront =
              new MyBusServiceManager(mac); //(new MyMachineManager());

            var result = await azureFront.Step1_InitAzure();


            Assert.True(result);
        }
    }
}