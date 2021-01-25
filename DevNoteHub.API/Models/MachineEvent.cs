using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNoteHub.Models
{
  public  class MachineEvent
    {

       

        //        [Id] int IDENTITY(1,1) NOT NULL
        //, [MachineId] nvarchar(4000) NOT NULL
        public int MachineId { get; set; }


        // [ClientId] nvarchar(4000) NULL
        public string ClientId { get; set; }

        //[TopicId] nvarchar(4000) NULL
        public string TopicId { get; set; }

        public bool IsDisabled { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string Domain { get; set; }
        public string Department { get; set; }

        //public string Alias { get; set; }

        public string Tag { get; set; }

        public string Descriptions { get; set; }




    }
}
