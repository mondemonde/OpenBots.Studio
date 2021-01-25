﻿using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Script;
using OpenBots.Core.UI.Controls.CustomControls;
using System.Collections.Generic;

namespace OpenBots.Core.Infrastructure
{
    public interface IfrmCommandEditor
    {
        List<AutomationCommand> CommandList { get; set; }
        List<ScriptVariable> ScriptVariables { get; set; }
        List<ScriptElement> ScriptElements { get; set; }
        string ProjectPath { get; set; }
        ScriptCommand SelectedCommand { get; set; }
        ScriptCommand OriginalCommand { get; set; }
        CreationMode CreationModeInstance { get; set; }
        string DefaultStartupCommand { get; set; }
        ScriptCommand EditingCommand { get; set; }
        List<ScriptCommand> ConfiguredCommands { get; set; }
        string HTMLElementRecorderURL { get; set; }
    }
}
