using System;
using System.Collections.Generic;
using System.Text;

namespace Steeroid.Models.Helpers
{
   public interface INote
    {
        void ClearLog();


        void LogError(Exception err);

        void LogError(string err);

        void LogInfo(string info);

        void LogWarn(string warn);
       
    }
}
