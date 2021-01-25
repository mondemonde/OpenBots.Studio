using DevNoteHub.Models;
using Steeroid.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNote.Interface.Models
{
  public  class WFProfileParameter:BaseModel
    {
       
     
        public string PropertyName { get; set; }
        public string MappedTo_Input_X { get; set; }

        public string DefaultValue { get; set; }

       // public int GlobalId { get; set; }


        public int? WFProfileId { get; set; }
        public int? WFProfileParameterId { get; set; }


        public int? GlobalMachineId { get; set; }


        public string GlobalFrontId
        {
            get
            {
                if (GlobalMachineId>0)
                {
                    return string.Format("{0}_{1}_{2}",Id.ToString(),WFProfileId.ToString(), GlobalMachineId);
                }
                return string.Empty;
            }
        }

        public string GlobalHubId
        {
            get
            {
                if (WFProfileParameterId.HasValue && WFProfileParameterId>0
                    && GlobalMachineId.HasValue && GlobalMachineId>0)
                {
                    return string.Format("{0}_{1}_{2}",WFProfileParameterId.ToString(), WFProfileId.ToString(), GlobalMachineId.ToString());
                }
                return string.Empty;
            }
        }

        public string Description { get; set; }

    }
}
