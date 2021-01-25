using System;
using System.Collections.Generic;
using System.Text;

namespace Steeroid.Models.Models
{
    public class MachineEvent
    {
       

        public int MachineId { get; set; }
        public string ClientId { get; set; }
        public string TopicId { get; set; }
        public bool IsDisabled { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Domain { get; set; }
        public string Department { get; set; }
        public string Tag { get; set; }
        public string Descriptions { get; set; }
    }

}
