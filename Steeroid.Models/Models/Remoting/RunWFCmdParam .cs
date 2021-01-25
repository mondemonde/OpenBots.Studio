using Steeroid.Models.Enums;
using Steeroid.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using DevNoteHub.Models;
//using taskt.DevNoteMod;
//using WFHostingApplication.COMMANDS;

namespace Steeroid.Models.Remoting
{
    //ActivityStep.1 CodeceptCmdParam
    public class RunWFCmdParam : CodeceptCmdParam,IBusMessage
    {
        #region Acitiviy Properties
        //public string XamlFullPath { get; set; }

        //public string username { get; set; }
        //public string password { get; set; }
        public Dictionary<string, string> EventParameters { get; set; }

        public string OuputResponse { get; set; }

      

        public int RetryCount { get; set; }

        public string ErrorCode { get; set; }


        #endregion

        string _id;
        public string GuidId
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        string _label;
        public string EventName { get { return _label; } set { _label = value; } }
        public string Label { get { return _label; } set { _label = value; } }
        public string MessageId
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }



        public string EventFilePath { get; set; }
        public string Content { get; set; }
        public string DomainName { get; set; }
        public int GlobalMachineId { get; set; }
        public bool IsResponse { get; set; }
        public string Mode { get; set; }
        public string ReferenceId { get; set; }
        public string Topic { get; set; }

        public RunWFCmdParam()
        {
            CommandName = EnumCmd.RunWF.ToString();
            //XamlFullPath = alias;           
            //self reference error 
            //Payload = this;
            SavePayload();
        }


        public new void SavePayload()
        {
            RequestDate = DateTime.Now;
            Payload = this.Payload;
        }
    }

}
