using Serilog;
using Steeroid.Models.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steeroid.Infra2._0.Helpers
{
    public class SLogger : INote
    {
        public const string LOGPATH = @"c:\BlastAsia\Steeroid\Log\Log.txt";

        #region ------------------------------------LOGGING---------------------
        public static void SetLogger(string logFile)
        {
            if (string.IsNullOrEmpty(logFile))
                logFile = LOGPATH;

            if (!Directory.Exists(Path.GetDirectoryName(logFile)))
                Directory.CreateDirectory(Path.GetDirectoryName(logFile));

            Log.Logger = new LoggerConfiguration()
                          .MinimumLevel.Debug()
                          .WriteTo.Console()
                          .WriteTo.File(logFile, rollingInterval: RollingInterval.Day)
                          .CreateLogger();
            _logger = Log.Logger;
        }

        //static DiagnosticLogger _logger;

        static ILogger _logger;

        public static ILogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    var dir = Agent.GetCurrentDir();
                    var logFile = Path.Combine(dir, "Logs\\Log.txt");

                    SetLogger(logFile);


                }

                return _logger;
            }

        }
        public  void ClearLog()
        {
            _logger = null;//new DiagnosticLogger();
        }
        public  void LogError(Exception err)
        {
            Logger.Error(err, err.Message);
        }
        public void LogError(string err)
        {
            Logger.Error(err);

        }
        public  void LogInfo(string info)
        {
            Logger.Information(info);
        }

        public  void LogWarn(string warn)
        {
            Logger.Warning(warn);
        }

     
        #endregion


    }

}
