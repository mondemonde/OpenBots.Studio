using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Steeroid.Business.Machine;
using Steeroid.Business.Services;
using Steeroid.Models.Enums;
using Steeroid.Models.Helpers;
using Steeroid.Models.Interfaces;
using System.Linq;
using Autofac;
using Steeroid.Models;
using Steeroid.Models.BaseInterfaces;
//using Steeroid.Models.Helpers;

namespace Steeroid.Business.Areas.Communication
{
    public abstract class BizBusServiceManager
    {
        #region CTOR

        IMachineManager _repo;
        private MachineServer _thisMachine;

        public BizBusServiceManager(IMachineManager repo)
        {
            _repo = repo;

        }

        public MachineServer ThisMachine
        {
            get
            {
                if(_thisMachine==null || _thisMachine.Id<1)
                {
                    _thisMachine = _repo.GetMachineRecord();
                }

                return _thisMachine;
            }
        }

        protected async Task<bool> IsOnline()
        {


            //_HACK safe to delete 
            #region---TEST ONLY: Compiler will  automatically erase this in RELEASE mode and it will not run if Global.GlobalTestMode is not set to TestMode.Simulation
#if OVERRIDE || OFFLINE

            System.Diagnostics.Debug.WriteLine("HACK-TEST -");
             return true;
            
#endif
            #endregion //////////////END TEST


            //STEP_.AUTOFAC #1  create scope
            var queueClient = GetQueueClient();
            string messageBody = $"Message check test.";

            //var message = new Microsoft.Azure.ServiceBus.Message(Encoding.UTF8.GetBytes(messageBody));
            // Write the body of the message to the console.
            Console.WriteLine($"Sending message: {messageBody}");
            var result = await queueClient.SendMessage(messageBody);

            if (result.HasError)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {result.ErrorMessages.FirstOrDefault()}");
                Agent.LogError(result.ErrorMessages.FirstOrDefault());

            }

            return result.Model;



            //try
            //{
            //    QueueClient queueClient = new QueueClient(svc, queueName, ReceiveMode.ReceiveAndDelete);
            //    // Create a new message to send to the queue.
            //    string messageBody = $"Message check test.";

            //    var message = new Microsoft.Azure.ServiceBus.Message(Encoding.UTF8.GetBytes(messageBody));
            //    // Write the body of the message to the console.
            //    Console.WriteLine($"Sending message: {messageBody}");
            //    // Send the message to the queue.
            //    await queueClient.SendAsync(message);
            //    await queueClient.CloseAsync();
            //    return true;
            //}

            //catch (Exception exception)
            //{
            //    Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            //    Agent.LogError(exception);
            //    return false;
            //}
        }

        protected  IBizQueueClient GetQueueClient()
        {

            //var serviceBusConnectionString = ThisMachine.ServiceBusConnectionString;// CryptoEngine.Decrypt(thisMac.ServiceBusConnectionString);
            ///var topicId = ThisMachine.TopicId;
            //var reader = scope.Resolve<ConfigReader>(new NamedParameter("configSectionName", "sectionName"));

            //STEP_.AUTOFAC #2 Resolve
            var queue =BizService.Resolve<IBizQueueClient>();

            

            //STEP_.AUTOFAC #3 update parameter to reconstruct self
            return  queue.CreateMe(ThisMachine);
          

        }

        #endregion

        public async Task<ServiceResponse<bool>> Step1_InitAzure()
        {

            //STEP_.AZURE_BUS #101 Init

            var config = new ConfigManager();
            //Global.MainWindow.MyConfig = config;
            Agent.LogInfo(config.GetValue("TestKey"));

            var model = false;
            ServiceResponse<bool> result = new ServiceResponse<bool>();
            result.AddModel(model);

            try
            {

                Console.WriteLine("DB initialize: test local db of Machine#" + ThisMachine.GlobalMacId);



                //STEP_.Designer #1 InitAzure              
                //decrypt..
                //var serviceBusConnectionString = CryptoEngine.Decrypt(thisMac.ServiceBusConnectionString);
                //var topicId = CryptoEngine.Decrypt(thisMac.TopicId);

                var isOnline = await IsOnline();//IsOnline(serviceBusConnectionString, topicId);


                if (!isOnline)
                {

                    return result;

                }

                //create
                var azureBus =  BizService.Resolve<IBizQueueClient>();
                //init wiht infra
                azureBus.CreateMe(ThisMachine);
                BizService.MyAzure = azureBus;

                Step2_CreateMessageHandler();
                model = true;

            }
            catch (Exception err)
            {

               Agent.LogError(err);
                result.AddValidationMessage(err.Message);
               model = false;
            }


            result.AddModel(model);
            return result;



        }

        /// <summary>
        ///  //Task MessageReceivedHandler(Message message, CancellationToken token);
        /// </summary>
        /// <returns></returns>
        public abstract  void Step2_CreateMessageHandler();

    }
}
