using DevNoteHub.Models;
using Steeroid.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNote.Interface.Models
{
  public  class WFProfile:BaseModel
    {
        public string Domain { get; set; }
        public string Department { get; set; }

        //public string Alias { get; set; }

        public string Tag { get; set; }

        public string Descriptions{ get; set; }
        

        public string SourcePath { get; set; }

        public bool InActive { get; set; }

        public int VersionNo { get; set; }


        public string Name
        {
            get
            {
                return string.Format("{0}__{1}__{2}__{3}", Domain, Department,FileName,VersionNo.ToString());
            }

        }


     //   public int GlobalId { get; set; }
        public int? WFProfileId { get; set; }

        public string GlobalFrontId {
            get
            {
                if(GlobalMachineId.HasValue && GlobalMachineId>0)
                {
                    return string.Format("{0}_{1}",  Id.ToString(), GlobalMachineId.ToString());
                }
                return null;
            }
        }



        public string GlobalHubId
        {
            get
            {
                if (WFProfileId.HasValue && WFProfileId > 0 
                   && GlobalMachineId.HasValue && GlobalMachineId > 0)
                {
                    return string.Format("{0}_{1}", WFProfileId.ToString(), GlobalMachineId.ToString());
                }
                return null;
            }
        }


        public string FileName
        {
            get
            {
                if (!string.IsNullOrEmpty(SourcePath))

                    return Path.GetFileNameWithoutExtension(SourcePath);
                else
                    return "";
            }

        }

        public int? GlobalMachineId { get; set; }

        [Obsolete]
        public string TenantId { get; set; }

        public string GuidId { get; set; }

        [NotMapped]
        public List<WFProfileParameter> Parameters { set; get; }



    }
}
