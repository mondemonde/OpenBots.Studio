//using DevNoteHub.Models;
using Steeroid.Models;
using Steeroid.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DevNoteHub.API.Models
{
   public class ResultMessage:BusMessage
    {
        public string ErrorCode { get; set; }
        public int RetryCount { get; set; }
        public Dictionary<string, string> EventParameters { get; set; }
        
        [MaxLength]
        public string OuputResponse 
        { get; set; }

        public string EventName { get; set; }

    }
}
