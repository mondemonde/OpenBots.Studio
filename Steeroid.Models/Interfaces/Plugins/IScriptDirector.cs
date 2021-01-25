using System;
using System.Collections.Generic;
//using System.Windows.Forms;
using System.Xml.Serialization;
using OpenBots.Core.Script;
//using taskt.Core.Script;
using Steeroid.Models.Scripting;

namespace Steeroid.Models.Interfaces.Plugins

{
    public interface IScriptDirector
    {
        List<Type> Types { get; set; }

        IScript MyDeserializeFile(string scriptFilePath);
        IScript MyDeserializeXML(string scriptXML);
        //Script SerializeScript(ListView.ListViewItemCollection scriptCommands, List<ScriptVariable> scriptVariables, string scriptFilePath = "");
        IScript MySerializeScript(object scriptCommands
            , List<IScriptVariable> scriptVariables, string scriptFilePath = "");

         T Clone<T>(T source);

        XmlSerializer GetSerializer();
    }
}