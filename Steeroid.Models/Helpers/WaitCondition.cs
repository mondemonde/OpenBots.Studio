using Steeroid.Models.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Steeroid.Models.Helpers
{
    public class WaitConditions
    {

        //public long Id { get; set; }

        //public int DevnotePort { get; set; }
        //public int ChromePort { get; set; }

        public string Name { get; set; }

        public WaitConditions()
        {
            // DevnotePort = Common.DefaultApiPort.DesignerPort;
            // ChromePort = Common.DefaultApiPort.ChromePort;
            Name = "TaskWaiter";
        }

        public WaitConditions(string name)
        {
            Name = name;
            //DevnotePort = Common.DefaultApiPort.MainPort;
            //ChromePort = Common.DefaultApiPort.ChromePort;
        }

        //public Conditions(int aPort, int cPort)
        //{
        //    DevnotePort = aPort;
        //    ChromePort = cPort;
        //}

        //todo use this queue
        //public Queue<int> Todos { get; set; }

        //public bool isChromeTaskDone { get; set; }

        //public bool IsDevBotRunning { get; set; }
        //public bool IsCodeCeptRunning { get; set; }
        //public bool IsFrontWfRunning { get; set; }



        //public async Task<string> WaitUntilChromeIsRunnung(int frequency = 100, int timeout = 45000)
        //{
        //    string result = string.Empty;
        //    isChromeTaskDone = false;

        //    var waitTask = Task.Run(async () =>
        //    {
        //        while (!isChromeTaskDone)
        //        {
        //            var response =  await BotHttpClient.TaskHttpGetToChrome("hi");

        //            if (response.StatusCode== System.Net.HttpStatusCode.OK)
        //            {
        //                isChromeTaskDone = true;
        //                var hello = response.Content.ReadAsStringAsync().Result;
        //                //Console.WriteLine(hello);
        //                BotHttpClient.WriteChromeResponse(hello);
        //                result = hello ;
        //            }                  

        //            if(GlobalDef.CurrentDesigner !=null && GlobalDef.CurrentDesigner.IsRunningWF==false)
        //            {
        //               var respond = "Cancelled Run WF.";
        //                BotHttpClient.WriteChromeResponse(respond);
        //                result = respond;
        //                isChromeTaskDone = true;
        //            }

        //            await Task.Delay(frequency);
        //        }
        //    });

        //    if (waitTask != await Task.WhenAny(waitTask,
        //            Task.Delay(timeout)))
        //    {
        //        // throw new TimeoutException();
        //        result = "WaitUntilChromeIsRunnung TimeoutException";
        //        LogApplication.Agent.LogError(result);
        //    }

        //    return result;
        //}

        //public async Task<string> WaitUntilCodeceptIsRunning(int frequency = 100, int timeout = 45000)
        //{
        //    string result = string.Empty;
        //    isChromeTaskDone = false;

        //    var waitTask = Task.Run(async () =>
        //    {
        //        while (!isChromeTaskDone)
        //        {
        //            var response = await BotHttpClient.TaskHttpGetToArmAPI("hi");

        //            if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //            {
        //                isChromeTaskDone = true;
        //                var hello = response.Content.ReadAsStringAsync().Result;
        //                //Console.WriteLine(hello);
        //                BotHttpClient.WriteChromeResponse(hello);
        //                result = hello;
        //            }

        //            await Task.Delay(frequency);
        //        }
        //    });

        //    if (waitTask != await Task.WhenAny(waitTask,
        //            Task.Delay(timeout)))
        //    {
        //        // throw new TimeoutException();
        //        result = "WaitUntilChromeIsRunnung TimeoutException";
        //        Agent.LogError(result);
        //    }

        //    return result;
        //}



        //public async Task<string> WaitUntilDesignerIsRunning(int frequency = 1000, int timeout = 60000)
        //{
        //    string result = string.Empty;
        //    bool isRunning = false;

        //    var waitTask = Task.Run(async () =>
        //    {
        //        while (!isRunning)
        //        {
        //            var response = await BotHttpClient.TaskHttpGetToDesigner("hi");

        //            if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //            {
        //                isRunning = true;
        //                var hello = response.Content.ReadAsStringAsync().Result;
        //                //Console.WriteLine(hello);
        //                BotHttpClient.WriteDevBotResponse(hello);
        //                result = hello;
        //            }

        //            await Task.Delay(frequency);
        //        }
        //    });

        //    if (waitTask != await Task.WhenAny(waitTask,
        //            Task.Delay(timeout)))
        //    {
        //        // throw new TimeoutException();
        //        result = "WaitUntilDesignerIsRunnung TimeoutException";
        //        Agent.LogError(result);
        //    }

        //    return result;
        //}


        public async Task<string> WaitUntilFileExist(string file, int frequency = 1000, int timeout = 20000)
        {
            string result = string.Empty;
            bool isExist = false;

            var waitTask = Task.Run(async () =>
            {
                int cnt = 0;
                while (!isExist)
                {
                    isExist = File.Exists(file);
                    if (cnt > 2)
                    {
                        Console.WriteLine("Busy..." + Name);
                        cnt = 0;
                    }
                    await Task.Delay(frequency);
                    cnt += 1;

                }
            });

            if (waitTask != await Task.WhenAny(waitTask,
                    Task.Delay(timeout)))
            {
                // throw new TimeoutException();
                result = "WaitUntilFileExist TimeoutException";
                //LogApplication.Agent.LogError(result);
            }

            return result;
        }

        public async Task<string> WaitUntil_File_Exist(string outputFile, int frequency = 1000, int timeout = 900000)
        {
            string result = string.Empty;
            bool isExist = false;

            var file = outputFile;

            var waitTask = Task.Run(async () =>
            {
                int cnt = 0;
                while (!isExist)
                {
                    isExist = File.Exists(file);
                    if (cnt > 2)
                    {
                        Console.WriteLine("Busy...waiting for Output.");
                        cnt = 0;
                    }
                    await Task.Delay(frequency);
                    cnt += 1;

                }
            });

            if (waitTask != await Task.WhenAny(waitTask,
                    Task.Delay(timeout)))
            {
                // throw new TimeoutException();
                result = "WaitUntil_OutputWF_Exist TimeoutException";
                //LogApplication.Agent.LogError(result);
            }

            return result;
        }


        //string CheckChromeStarted()
        //{
        //    string result = string.Empty;
        //    var response =  BotHttpClient.TaskHttpGetToChrome("hi").Result;

        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        isChromeTaskDone = true;
        //        var hello = response.Content.ReadAsStringAsync().Result;
        //        //Console.WriteLine(hello);
        //        BotHttpClient.WriteChromeResponse(hello);
        //        result = hello;
        //    }
        //    return result;

        //}







        /// <summary>
        /// Blocks while condition is true or timeout occurs.
        /// </summary>
        /// <param name="condition">The condition that will perpetuate the block.</param>
        /// <param name="frequency">The frequency at which the condition will be check, in milliseconds.</param>
        /// <param name="timeout">Timeout in milliseconds.</param>
        /// <exception cref="TimeoutException"></exception>
        /// <returns></returns>
        public async Task WaitWhile(Func<bool> condition, int frequency = 100
            , int timeout = -1)
        {
            var waitTask = Task.Run(async () =>
            {
                while (condition()) await Task.Delay(frequency);
            });

            if (waitTask != await Task.WhenAny(waitTask, Task.Delay(timeout)))
                throw new TimeoutException();
        }

        /// <summary>
        /// Blocks until condition is true or timeout occurs.
        /// </summary>
        /// <param name="condition">The break condition.</param>
        /// <param name="frequency">The frequency at which the condition will be checked.</param>
        /// <param name="timeout">The timeout in milliseconds.</param>
        /// <returns></returns>
        public async Task WaitUntil(Func<bool> condition, int frequency = 200, int timeout = -1)
        {
            var waitTask = Task.Run(async () =>
            {
                bool isconsole = true;
                DateTime dateStatus = DateTime.Now.AddSeconds(5);
                while (!condition())
                {

                    await Task.Delay(frequency);


                    if (string.IsNullOrEmpty(this.Name))
                    {
                        this.Name = "WaitUntil";
                    }
                    if (dateStatus < DateTime.Now)
                    {
                        try
                        {
                            dateStatus = DateTime.Now.AddSeconds(10);
                            Console.WriteLine("Busy... " + Name);

                        }
                        catch (Exception)
                        {

                            //throw;
                        }
                    }
                    try
                    {
                        if (isconsole)
                            ConsoleSpinner.Instance.Update();

                    }
                    catch (Exception)
                    {
                        //throw;
                        isconsole = false;
                    }
                }
            });

            if (waitTask != await Task.WhenAny(waitTask,
                    Task.Delay(timeout)))
                throw new TimeoutException();
        }
    }

}
