using Steeroid.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Steeroid.Models
{
    public class DevNoteCmdEvent : IBusMessage
    {
        public DevNoteCmdEvent(){
            GuidId = Guid.NewGuid().ToString();
            EventParameters = new Dictionary<string, string>();
        }   

        public DevNoteCmdEvent(Guid id, 
            string eventName, Dictionary<string, string> parameters)
        {
            //GuidId = id;
            GuidId = id.ToString();
            EventParameters = parameters;
            EventName = eventName;//vl
        }

        string _id;
       

        public  string GuidId {
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


        public string OuputResponse { get; set; }

        public Dictionary<string, string> EventParameters { get; set; }

        public int RetryCount { get; set; }

        public string ErrorCode { get; set; }

        public string Mode { get; set; }
        public string Topic { get; set; }
        public string Content { get; set; }
        public string DomainName { get; set; }
        //public int GlobalMachineId {get;set;}
        public bool IsResponse { get; set; }

      
public string ReferenceId { get; set; }
        public int GlobalMachineId { get; set; }
    }
}
