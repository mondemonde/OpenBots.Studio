using Steeroid.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Steeroid.Models
{
    public class BusMessage : BaseModel,IBusMessage
    {

        public string Topic { get; set; }
        public string Label { get; set; }
        public string Content { get; set; }
        public string MachineId { get; set; }
        public string Mode { get; set; }
        public string MessageId { get; set; }
        public bool IsResponse { get; set; }
        public int GlobalMachineId { get; set; }
        public string DomainName { get; set; }
        public string ReferenceId { get; set; }
    }
}
