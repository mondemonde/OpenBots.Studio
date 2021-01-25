//using CsvHelper;
//using ImapX.Extensions;
//using IntegrationEvents.Events.DevNote;
//using mshtml;
using Newtonsoft.Json;
using Steeroid.Business.Services;
using Steeroid.Models;
using Steeroid.Models.Enums;
//using QR.API.Mapper.Std;
//using SimpleNLG.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using System.Web.Http.Results;
//using taskt.UI.Forms;

namespace Steeroid.Business.Areas.Output
{
    /// <summary>
    /// based on C:\Users\rgalvez\source\DevOpsRepos\steeroid\Tasktskie.Common\DevNoteMod\Output\OutputManager.cs
    /// </summary>
    public static   class OutputManager
    {
        //public static frmScriptEngine CurrentBot { get; set; }

        public static CancellationTokenSource cancellationSource { get; set; }
        public static CancellationToken cancellationToken { get; set; }

        public static string ErrorMessage { get; set; }
        public static string Result {

            get {


                return File.ReadAllText(FileEndPointManager.DefaultLatestResultFile);
            
            }

            set
            {
                File.WriteAllText(FileEndPointManager.DefaultLatestResultFile, value);
            }
        
        }

        public static void ClearResult()
        {
            if(File.Exists(FileEndPointManager.DefaultLatestResultFile))
            {
                File.Delete(FileEndPointManager.DefaultLatestResultFile);
            }
        }

        public static DevNoteIntegrationEvent Output { get; set; }

        public static bool IsTasktAgentRunning { get; set; }

        public static ErrorCodes ThrowErrorOnResult()
        {
            var result = ErrorCodes.None;
            var resultFile = FileEndPointManager.DefaultLatestResultFile;
            if (File.Exists(resultFile))
            {
                var txt=  File.ReadAllText(resultFile);

               
                if (txt.Length > 1)
                {

                    if (txt == "Katalon Failed"
                   || txt == ErrorCodes.ErrorOnKatalon.ToString())
                    {
                        result = ErrorCodes.ErrorOnKatalon;

                    }

                    var split = txt.Trim().Split();
                    if(split.Length>0)
                    {
                      if( Enum.TryParse(split[0], out ErrorCodes enumErr))
                                        {
                                             //STEP_.RESULT #10 by convention error message starts with ErrorOn
                                            if(enumErr.ToString().StartsWith("ErrorOn"))
                                               result = enumErr;
                                        }
                    }
                 

                }


            }

            return result;
        }

        public static void ThrowError(ErrorCodes err)
        {
            if(err!=ErrorCodes.None)
            {
                throw new Exception(err.ToString());
            }
        }

        public static void ThrowError(string err)
        {           
                throw new Exception(err);
           
        }



        public static bool CreateErrorResult(string ErrorMessage)
        {
            var resultFile = FileEndPointManager.DefaultLatestResultFile;
            File.WriteAllText(resultFile, ErrorMessage.Trim());           
            return true;
        }

        public static async Task<bool> CreateOutputWF()
        {
            
            Output = await FileEndPointManager.CreateOutputWF();
            return true;
        }

        public static async Task<bool> CreateErrorOutputWF(string message)
        {
            //STEP_.RESULT #901
            ////1.remove wfinput from event folder and create WFOUPUT file....
            await FileEndPointManager.CreateErrorOutputWF(message);
            //FileEndPointManager.ClearOutputWF();

            OutputManager.ErrorMessage = message;
            

            return true;
        }

        public static bool IsAutoRun
        {
            get
            {
                return IsTasktAgentRunning;
            }
        }

        //STEP_.RESULT #11 Only set once on frmScriptEngine.frmProcessingStatus_Load
        public static bool IsTasktDone { get; set; }

        //STEP_.RESULT #11 Only set once on RunCommand and LogController
        //used for external apps like excel
        public static bool IsReadyForNext { get; set; }



        //STEP_.RESULT #11 Only set once on frmScriptEngine.frmProcessingStatus_Load
        public static bool IsPollyDone { get; set; }

        #region CSV
        public static DataTable jsonStringToTable(string jsonContent)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonContent);
            return dt;
        }

        //public static string jsonToCSV(string jsonContent, string delimiter)
        //{
        //    StringWriter csvString = new StringWriter();



        //    using (var csv = new CsvWriter((csvString))
        //    {
        //        csv.Configuration..SkipEmptyRecords = true;
        //        csv.Configuration.WillThrowOnMissingField = false;
        //        csv.Configuration.Delimiter = delimiter;

        //        using (var dt = jsonStringToTable(jsonContent))
        //        {
        //            foreach (DataColumn column in dt.Columns)
        //            {
        //                csv.WriteField(column.ColumnName);
        //            }
        //            csv.NextRecord();

        //            foreach (DataRow row in dt.Rows)
        //            {
        //                for (var i = 0; i < dt.Columns.Count; i++)
        //                {
        //                    csv.WriteField(row[i]);
        //                }
        //                csv.NextRecord();
        //            }
        //        }
        //    }
        //    return csvString.ToString();
        //}
        #endregion


    }
}
