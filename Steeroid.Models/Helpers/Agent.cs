using System;
using System.Collections.Generic;
using System.Text;

namespace Steeroid.Models.Helpers
{
    public static class Agent
    {

        static INote _logger;

        public static INote Logger => _logger;

        public static void SetLogger(INote _note)
        {
            _logger = _note;
        }


        public static void ClearLog()
        {
            _logger.ClearLog();
        }


        public static void LogError(Exception err)
        {
            Logger.LogError(err);
        }
        public static void LogError(string err)
        {
            Logger.LogError(err);

        }
        public static void LogInfo(string info)
        {
            Logger.LogInfo(info);
        }

        public static void LogWarn(string warn)
        {
            Logger.LogWarn(warn);


        }

        public static string GetCurrentDir()
        {
            var dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            dir = dir.Replace("file:\\", string.Empty);
            return dir;
        }



        #region TEST HELPER
        private static List<TestToken> _test;
        private static bool _enableTestLog;

        public static bool EnableTestLog { get=>_enableTestLog; }

        public static List<TestToken> Test { get => _test; }

        //private static string testResult;
        //public static string TestResult { get => testResult; }
        public static void Retest()
        {
            //testResult = string.Empty;
            _test = new List<TestToken>();
            _enableTestLog = true;
        }

        public static void LogTest(string testmarker)
        {
            if(_enableTestLog)
            {
                _test.Add(new TestToken
                {

                    Name = testmarker,
                    Time = DateTime.Now.ToUniversalTime()

                });

            }

            //testResult = message;
        }

        public static void DisableTest()
        {
            //testResult = string.Empty;
            _test = new List<TestToken>();
            _enableTestLog = false;
        }


        #endregion
    }

}
