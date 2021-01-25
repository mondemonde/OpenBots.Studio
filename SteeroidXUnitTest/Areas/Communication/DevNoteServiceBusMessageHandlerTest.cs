using Autofac;
using Autofac.Extras.Moq;
using Microsoft.Azure.ServiceBus;
using Moq;
using Newtonsoft.Json;
using Steeroid.Business.Areas.Bot;
using Steeroid.Business.Areas.Input;
using Steeroid.Business.Machine;
using Steeroid.Business.Services;
using Steeroid.Infra2._0.Areas.Communication;
using Steeroid.Models;
using Steeroid.Models.Enums;
using Steeroid.Models.Helpers;
using SteeroidXUnitTest;
using SteeroidXUnitTest.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Xunit;

namespace Steeroid.Infra2._0.Areas.Communication.Tests
{
    [Collection("Non-Parallel Collection")]

    public class DevNoteServiceBusMessageHandlerTest:BaseFixture
    {
        [Fact]
        public async void MessageReceivedHandler()
        {
            Agent.Retest();
            var manager = MyFactory.Resolve<IMachineManager>();
            var mac = manager.GetMachineRecord();

            //clear input files
            //var eventFiles = Directory.GetFiles(FileEndPointManager.MyEventDirectory
            //   , "*" + EnumFiles.WFInput.ToString(), SearchOption.TopDirectoryOnly)
            //   .ToList().OrderBy(x => x);

            //foreach (var item in eventFiles)
            //{
            //    File.Delete(item);
            //}

            //eventFiles = Directory.GetFiles(FileEndPointManager.MyEventDirectory
            //  , "*" + EnumFiles.WFInput.ToString(), SearchOption.TopDirectoryOnly)
            //  .ToList().OrderBy(x => x);

            //Assert.True(eventFiles.Count() == 0, "input file json folder is not cleared.");
            FileTestHelper.ClearInputFiles();

         
            //create bus Message
            var client = new MyQueueClient(mac);// //mock.Create<MyQueueClient>();
            var serviceBusHandler = new DevNoteServiceBusMessageHandler(client.MyAzureClient);

            // read file into a string and deserialize JSON to a type
            var dir =  Agent.GetCurrentDir();
            var file = Path.Combine(dir, @"Resources\TestMessage.json");
            Message msg = JsonConvert.DeserializeObject<Message>(File.ReadAllText(file));
            msg.MessageId = Guid.NewGuid().ToString();

            var message = msg;//new Message();
            //message.TimeToLive = TimeSpan.FromSeconds(10);
            var systemProperties = new Message.SystemPropertiesCollection();

            // systemProperties.EnqueuedTimeUtc = DateTime.UtcNow.AddMinutes(1);
            var bindings = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty;
            var value = DateTime.UtcNow.AddMinutes(1);
            systemProperties.GetType().InvokeMember("EnqueuedTimeUtc", bindings, Type.DefaultBinder, systemProperties, new object[] { value });
            // workaround "ThrowIfNotReceived" by setting "SequenceNumber" value
            systemProperties.GetType().InvokeMember("SequenceNumber", bindings, Type.DefaultBinder, systemProperties, new object[] { 1 });

            // message.systemProperties = systemProperties;
            bindings = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty;
            message.GetType().InvokeMember("SystemProperties", bindings, Type.DefaultBinder, message, new object[] { systemProperties });

            //act         
            CancellationToken token = new CancellationToken();
            var resposne = await serviceBusHandler.MessageReceivedHandler(message, token);


            //assert
            Assert.True(Agent.Test.Exists(x=>x.Name =="DEVFRONT_RUN"),"Automation is not triggered.");

            var eventFiles = Directory.GetFiles(FileEndPointManager.MyEventDirectory
               , "*" + EnumFiles.WFInput.ToString(), SearchOption.TopDirectoryOnly)
               .ToList().OrderBy(x => x);

            //input file should exist
            Assert.True(eventFiles.Count() > 0,"input file json is missing.");


            //using (var mock = AutoMock.GetLoose())
            //{
            //    var mac = mock.Mock<MachineServer>();//.Setup(x => { x.GlobalMacId = 1; }).Returns("expected value");
            //    mac.SetupProperty(x => x.GlobalMacId, 1);
            //    mac.SetupProperty(x => x.ServiceBusConnectionString, GlobalTest.ServiceBusConnectionString); ;

            //    var client = mock.Create<MyQueueClient>();
            //    var serviceBusHandler = new DevNoteServiceBusMessageHandler(client.MyAzureClient);


            //}
        }
    }
}