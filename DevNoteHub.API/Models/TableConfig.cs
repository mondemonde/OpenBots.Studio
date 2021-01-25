using DevNoteHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNote.Interface.Models
{
    public class TableConfig 
    {
        public int Id { get; set; }

        //[MaxTry] int DEFAULT((3)) NOT NULL
        public int MaxTry { get; set; }
        //, [IsJWTPrivateKeyValidate] bit DEFAULT((0)) NOT NULL
        public bool IsJWTPrivateKeyValidate { get; set; }
        public bool Enable_JWTExpiration { get; set; }

//, [EmailTemplateForFirstRegister] nvarchar(4000) NOT NULL
//, [WebTitle] nvarchar(200) NOT NULL


    }

}
