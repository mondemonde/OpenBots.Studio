using LogApplication.INFRA;
using Serilog;
using Steeroid.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogApplication
{

   static class thisAgent
    {
        static ILogger _logger;
        public static ILogger Write
        {
            get
            {
                if (_logger == null)
                {
                    Log.Logger = new LoggerConfiguration()
                           .MinimumLevel.Debug()
                           .WriteTo.Console()
                           .WriteTo.File("c:\\BlastAsia\\UpdateManager\\Log.txt"
                           , rollingInterval: RollingInterval.Day)
                           .CreateLogger();
                    _logger = Log.Logger;
                }

                return _logger;
            }
        }
        //{


        //    Log.Information("Hello, world!");
        //}
    }


}
