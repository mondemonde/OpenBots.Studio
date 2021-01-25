using Steeroid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steeroid.Models
{
   public class TableConfig:BaseModel
    {
        // public  DateTime BaiCalendarLastUpdate { get; set; }
        // public string BaiAccessToken { get; set; }

        //public int BatchNo { get; set; }
        //public int RecPerBatch { get; set; }

        public string Title { get; set; }
        public string Url { get; set; }

       public bool Enable_JWTExpiration { get; set; }
       public bool IsJWTPrivateKeyValidate { get; set; }

        public int MaxTry { get; set; }
       


    }
}
