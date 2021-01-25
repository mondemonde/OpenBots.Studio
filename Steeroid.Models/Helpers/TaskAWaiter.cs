//using Common;
//using LogApplication.Common;
using Steeroid.Models.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Steeroid.Models.Helpers
{
    public enum EnumTaskStatus
    {
        Started,
        DoneCodeCept, 
        DoneBGWF,
        Finished
    }
    public class TaskAwaiter
    {

       public long Id { get; set; }
        public string Name { get; set; }
        public  TaskAwaiter()
        {
            Name = "TaskWaiter";
        }

        public TaskAwaiter(string name)
        {
            Name = name;
        }           

        /// <summary>
        /// Blocks while condition is true or timeout occurs.
        /// </summary>
        /// <param name="condition">The condition that will perpetuate the block.</param>
        /// <param name="frequency">The frequency at which the condition will be check, in milliseconds.</param>
        /// <param name="timeout">Timeout in milliseconds.</param>
        /// <exception cref="TimeoutException"></exception>
        /// <returns></returns>
        public  async Task WaitWhile(Func<bool> condition, int frequency = 100
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
        public  async Task WaitUntil(Func<bool> condition, int frequency = 100, int timeout = -1)
        {
            var waitTask = Task.Run(async () =>
            {
                bool isconsole = true;
                DateTime dateStatus = DateTime.Now.AddSeconds(5);
                while (!condition())
                {
                    
                    await Task.Delay(frequency);
                 
                    if(string.IsNullOrEmpty(this.Name))
                    {
                        this.Name = "WaitUntil";
                    }
                    if (dateStatus < DateTime.Now)
                    {
                        dateStatus = DateTime.Now.AddSeconds(10);
                        Console.WriteLine("Busy... " + Name);
                    }
                    try
                    {
                        if(isconsole)
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
