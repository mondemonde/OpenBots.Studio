using System;
using System.Collections.Generic;
using System.Text;

namespace Steeroid.API.Models
{
  public  class EventRecord
    {
        public string EventTag { get; set; }
        public int MachineNo { get; set; }

        public string ReferenceId  { get; set; }
        public string ApiKey { get; set; }

        public Dictionary<string,string> Parameters { get; set; }



    }
}
