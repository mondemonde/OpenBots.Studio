using OpenBots.Core.Script;
using Steeroid.Models.Scripting;
using System.Collections.Generic;
//using taskt.Core.Script;

namespace Steeroid.Models.Interfaces.Scripting

{
    public interface IScriptManager
    {
        string CurrentHead { get; set; }
        Dictionary<string, string> ExtVars { get; set; }
        IScript MyScript { get; }
        List<IScriptVariable> MyScriptVariables { get; set; }
        string SourceXmlFile { get; set; }

        Dictionary<string, string> ReadWriteXml(string xml, Dictionary<string, string> extParam);
    }
}