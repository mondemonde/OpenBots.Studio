using Steeroid.Models.Scripting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Steeroid.Infra2._0.Areas.Recordings
{
    public class ScriptAction : IScriptAction
    {
        /// <summary>
        /// generic 'top-level' user-defined script command (ex. not nested)
        /// </summary>
        [XmlElement(Order = 1)]
        public ScriptCommand ScriptCommand { get; set; }
        /// <summary>
        /// generic 'sub-level' commands (ex. nested commands within a loop)
        /// </summary>
        [XmlElement(Order = 2)]
        public List<ScriptAction> AdditionalScriptCommands { get; set; }
        /// <summary>
        /// adds a command as a nested command to a top-level command
        /// </summary>
        public ScriptAction AddAdditionalAction(IScriptCommand scriptCommand)
        {
            if (AdditionalScriptCommands == null)
            {
                AdditionalScriptCommands = new List<ScriptAction>();
            }

            ScriptAction newExecutionCommand = new ScriptAction() { ScriptCommand = scriptCommand };
            AdditionalScriptCommands.Add(newExecutionCommand);
            return newExecutionCommand;
        }
    }

}
