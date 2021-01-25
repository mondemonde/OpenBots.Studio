//using DevNoteHub.Models;
//using IntegrationEvents.Events.DevNote;
using Steeroid.Models;
using Steeroid.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Steeroid.Models
{
    public class EventDTO: DevNoteCmdEvent,IBusMessage
    {
        public EventDTO():base()
        {
            
        }

        public WFProfile Event { get; set; }
        public List<WFProfileParameter> Params { get; set; }

      
    }
}
