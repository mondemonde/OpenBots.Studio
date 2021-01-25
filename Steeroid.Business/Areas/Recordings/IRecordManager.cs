using Steeroid.Business.Areas.Remoting;
using Steeroid.Models.Interfaces;
using Steeroid.Models.Interfaces.Scripting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Steeroid.Business.Areas.Recordings
{
   public interface IRecordManager
    {

        IRepository DBAccess {get;}
        ISynchManager SynchManager { get; }
        IScriptManager ScriptManager { get; }

    }
}
