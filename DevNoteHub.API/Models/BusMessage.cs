using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevNoteHub.Models
{
    public class BusMessage : BaseModel//, IBusMessage
    {
//        [Id] int IDENTITY(1,1) NOT NULL
//, [Topic] nvarchar(4000) NULL
//, [Label] nvarchar(4000) NULL
//, [Content] nvarchar(4000) NULL
//, [Created] datetime NOT NULL
//, [Modified]
//        datetime NULL
//, [MachineId] nvarchar(4000) NULL
//, [Mode] nvarchar(100) NULL
//, [MessageId] nvarchar(2000) NULL
//, [IsResponse] bit DEFAULT(((0))) NOT NULL
//, [GlobalMachineId] int DEFAULT((0)) NOT NULL
//, [DomainName] nvarchar(1000) NULL
//, [ReferenceId] nvarchar(1000) NULL

        public string Topic { get; set; }//
        public string Label { get; set; }//
        public string Content { get; set; }//

        public string MachineId { get; set; }//
        public string Mode { get; set; }//
        public string MessageId { get; set; }//
        public bool IsResponse { get; set; }//

        public int GlobalMachineId { get; set; }//
        public string DomainName { get; set; }//
        public string ReferenceId { get; set; }//
     

    }
}