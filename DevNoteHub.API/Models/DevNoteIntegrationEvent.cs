using DevNoteHub.Models;
using EFCoreTransactionsReceiver.IntegrationEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationEvents.Events.DevNote
{
    public class DevNoteIntegrationEvent : IBusMessage
    {
        public DevNoteIntegrationEvent()
        {
            //GuidId = Guid.NewGuid().ToString();
            EventParameters = new Dictionary<string, string>();
        }

        public DevNoteIntegrationEvent(Guid id,
            string eventName, Dictionary<string, string> parameters)
        {
            //GuidId = id;
            GuidId = id.ToString();
            EventParameters = parameters;
            EventName = eventName;//vl
        }

        string _id;
        //public new Guid Id
        //{
        //    get
        //    {
        //        return _id;
        //    }
        //    set
        //    {
        //        _id = value;
        //    }
        //}

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
        public string OuputResponse { get; set; }
        public Dictionary<string, string> EventParameters { get; set; }
        public int RetryCount { get; set; }
        public string ErrorCode { get; set; }
        public string Content { get; set; }
        public string DomainName { get; set; }
        public int GlobalMachineId { get; set; }
        public bool IsResponse { get; set; }
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
        public string Mode { get; set; }
        public string ReferenceId { get; set; }
        public string Topic { get; set; }
    }
}
