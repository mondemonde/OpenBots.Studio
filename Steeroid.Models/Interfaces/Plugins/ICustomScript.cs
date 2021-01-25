using Steeroid.Models.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using taskt.Core.Script;

namespace Steeroid.Models.Interfaces.Plugins
{
    public interface ICustomScript
    {
        Dictionary<string, string> MyGetCustomVars(IScriptAction action);
        void MySetCustomVars(IScriptAction action);


    }
}
