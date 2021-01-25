using OpenBots.Core.Script;
using Steeroid.Models.Interfaces.Scripting;
using Steeroid.Models.Scripting;
using System.Collections.Generic;

namespace Steeroid.Models.Scripting
{
    public interface IScript
    {
        List<IScriptVariable> MyVariables { get; set; }

        IScriptAction MyAddNewParentCommand(IScriptCommand scriptCommand);
    }
}